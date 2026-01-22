using Core.Enums;
using Core.Interfaces.Services;
using Core.Models.Match;
using Microsoft.AspNetCore.SignalR;
using RestAPI.Hubs;
using System.Collections.Concurrent;

namespace RestAPI.Services
{
    public class MatchTimer : IMatchTimer
    {
        private readonly ConcurrentDictionary<Guid, PlayerTimer> _timers;
        private readonly IHubContext<MatchHub> _hubContext;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public MatchTimer(IHubContext<MatchHub> hubContext,
            IServiceScopeFactory serviceScopeFactory)
        {
            _timers = new ConcurrentDictionary<Guid, PlayerTimer>();
            _hubContext = hubContext;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public double StartTurnTimer(GameType gameType, Guid matchId, Guid playerId)
        {
            PlayerTimer timer;
            var duration = GetTurnDuration(gameType);

            if (_timers.ContainsKey(playerId))
            {
                timer = _timers[playerId];
                duration = timer.RemainingTime;
                timer.DeadlineDateTime = DateTime.UtcNow + duration;
            }
            else
            {
                timer = new PlayerTimer
                {
                    DeadlineDateTime = DateTime.UtcNow + duration,
                    RemainingTime = duration
                };

                _timers[playerId] = timer;
            }

            timer.Cts = new CancellationTokenSource();

            _ = Task.Run(async () =>
            {
                try
                {
                    await Task.Delay(duration, timer.Cts.Token);

                    _timers.TryRemove(playerId, out _);

                    using var scope = _serviceScopeFactory.CreateScope();
                    var matchService = scope.ServiceProvider.GetRequiredService<IMatchService>();

                    // await matchService.EndMatchAsync(matchId);
                }
                catch (TaskCanceledException)
                {
                }
            });

            return timer.RemainingTime.TotalSeconds;
        }

        public void PauseTurnTimer(GameType gameType, Guid playerId)
        {
            if (!_timers.TryGetValue(playerId, out var timer))
                return;

            timer.Cts.Cancel();

            if (gameType == GameType.Chess)
            {
                timer.RemainingTime = timer.DeadlineDateTime - DateTime.UtcNow;
            }
            else
            {
                _timers.TryRemove(playerId, out _);
            }
        }

        public double StartMatchCountdown(GameType gameType, Guid matchId)
        {
            if (!_timers.TryGetValue(matchId, out var timer))
            {
                timer = new PlayerTimer()
                {
                    DeadlineDateTime = DateTime.UtcNow.AddMinutes(5),
                    RemainingTime = TimeSpan.FromMinutes(5),
                    Cts = new CancellationTokenSource()
                };

                _timers[matchId] = timer;
            }

            _ = Task.Run(async () =>
            {
                await Task.Delay(timer.RemainingTime, timer.Cts.Token);

                _timers.TryRemove(matchId, out _);

                await _hubContext.Clients.Group($"{gameType}")
                    .SendAsync("RemoveMatch", matchId);

                await _hubContext.Clients.Group($"{matchId}")
                    .SendAsync("EndMatch", matchId);

                using var scope = _serviceScopeFactory.CreateScope();
                var matchService = scope.ServiceProvider.GetRequiredService<IMatchService>();

                //await matchService.EndMatchAsync(matchId);
            });

            return timer.RemainingTime.TotalSeconds;
        }

        public void RemoveTimer(Guid matchId)
        {
            _timers.TryRemove(matchId, out _);
        }

        public double GetActiveTimer(Guid key)
        {
            double timeLeft = 0;

            if (_timers.TryGetValue(key, out var time))
            {
                timeLeft = time.RemainingTime.TotalSeconds;
            }

            return timeLeft;
        }

        private TimeSpan GetTurnDuration(GameType gameType)
        {
            return gameType switch
            {
                GameType.Chess => TimeSpan.FromMinutes(10),
                _ => TimeSpan.FromMinutes(1),
            };
        }
    }
}
