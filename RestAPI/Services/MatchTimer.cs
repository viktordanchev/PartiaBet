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

        public double StartTurnTimer(GameType gameType, Guid matchId, Guid palyerId)
        {
            PlayerTimer timer;
            var duration = GetTurnDuration(gameType);

            if (_timers.ContainsKey(palyerId))
            {
                timer = _timers[palyerId];
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

                _timers[palyerId] = timer;
            }

            timer.Cts = new CancellationTokenSource();

            _ = Task.Run(async () =>
            {
                try
                {
                    await Task.Delay(duration, timer.Cts.Token);

                    using var scope = _serviceScopeFactory.CreateScope();
                    var matchService = scope.ServiceProvider.GetRequiredService<IMatchService>();

                    await matchService.EndMatchAsync(matchId);
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

            if(gameType == GameType.Chess)
            {
                timer.RemainingTime = timer.DeadlineDateTime - DateTime.UtcNow;
            }
            else
            {
                _timers.TryRemove(playerId, out _);
            }
        }

        public void StartLeaverTimer(GameType gameType, Guid matchId, Guid palyerId)
        {
            if(!_timers.TryGetValue(palyerId, out var timer))
            {
                timer = new PlayerTimer()
                {
                    DeadlineDateTime = DateTime.UtcNow.AddMinutes(5),
                    RemainingTime = TimeSpan.FromMinutes(5),
                    Cts = new CancellationTokenSource()
                };

                _timers[palyerId] = timer;
            }

            _ = Task.Run(async () =>
            {
                await _hubContext.Clients.Group($"{matchId}")
                        .SendAsync("TimeLeftToRejoin", palyerId, timer.RemainingTime.TotalSeconds);

                await Task.Delay(timer.RemainingTime, timer.Cts.Token);

                await _hubContext.Clients.Group($"{gameType}")
                    .SendAsync("RemoveMatch", matchId);

                await _hubContext.Clients.Group($"{matchId}")
                    .SendAsync("EndMatch", matchId);

                using var scope = _serviceScopeFactory.CreateScope();
                var matchService = scope.ServiceProvider.GetRequiredService<IMatchService>();

                await matchService.EndMatchAsync(matchId);
            });
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
