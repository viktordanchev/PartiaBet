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
        private readonly ConcurrentDictionary<Guid, CancellationTokenSource> _timers;
        private readonly IHubContext<MatchHub> _hubContext;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public MatchTimer(IHubContext<MatchHub> hubContext,
            IServiceScopeFactory serviceScopeFactory)
        {
            _timers = new ConcurrentDictionary<Guid, CancellationTokenSource>();
            _hubContext = hubContext;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public void StartTurnTimer(PlayerModel player)
        {
            var cts = new CancellationTokenSource();
            _timers[player.Id] = cts;

            _ = Task.Run(async () =>
            {
                try
                {
                    await Task.Delay(player.Timer.TimeLeft, cts.Token);

                    _timers.TryRemove(player.Id, out _);

                    using var scope = _serviceScopeFactory.CreateScope();
                    var matchService = scope.ServiceProvider.GetRequiredService<IMatchService>();

                    // await matchService.EndMatchAsync(matchId);
                }
                catch (TaskCanceledException)
                {
                }
            });
        }

        public void PauseTurnTimer(PlayerModel player)
        {
            if (!_timers.TryGetValue(player.Id, out var timer))
                return;

            timer.Cancel();
            timer.Dispose();
        }

        public void StartMatchCountdown(GameType gameType, Guid matchId, TimeSpan countdown)
        {
            var cts = new CancellationTokenSource();
            _timers[matchId] = cts;

            _ = Task.Run(async () =>
            {
                await Task.Delay(countdown, cts.Token);

                _timers.TryRemove(matchId, out _);

                await _hubContext.Clients.Group($"{gameType}")
                    .SendAsync("RemoveMatch", matchId);

                await _hubContext.Clients.Group($"{matchId}")
                    .SendAsync("EndMatch", matchId);

                using var scope = _serviceScopeFactory.CreateScope();
                var matchService = scope.ServiceProvider.GetRequiredService<IMatchService>();

                //await matchService.EndMatchAsync(matchId);
            });
        }

        public void RemoveTimer(Guid key)
        {
            if (_timers.TryRemove(key, out var cts))
            {
                cts.Cancel();
                cts.Dispose();
            }
        }
    }
}
