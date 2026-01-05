using Core.Enums;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.SignalR;
using RestAPI.Hubs;
using System.Collections.Concurrent;

namespace RestAPI.Services
{
    public class MatchTimer : IMatchTimer
    {
        private readonly ConcurrentDictionary<Guid, CancellationTokenSource> _timers;
        private readonly IHubContext<MatchHub> _hubContext;

        public MatchTimer(IHubContext<MatchHub> hubContext)
        {
            _timers = new ConcurrentDictionary<Guid, CancellationTokenSource>();
            _hubContext = hubContext;
        }

        public void StartMoveTimer() 
        {

        }

        public void StartLeaverTimer(Guid matchId, Guid palyerId)
        {
            StopTimer(palyerId);

            var cts = new CancellationTokenSource();
            _timers[palyerId] = cts;

            var duration = TimeSpan.FromMinutes(5);

            _ = Task.Run(async () =>
            {
                try
                {
                    await _hubContext.Clients.Group($"{matchId}")
                        .SendAsync("TimeLeftToRejoin", palyerId, duration.TotalSeconds);

                    await Task.Delay(duration, cts.Token);

                    if (!cts.Token.IsCancellationRequested)
                    {
                        await _hubContext.Clients.Group($"{GameType.Chess}")
                        .SendAsync("Remove", palyerId);
                    }
                }
                catch (TaskCanceledException) { }
            });
        }

        public void StopTimer(Guid matchId)
        {
            if (_timers.TryRemove(matchId, out var cts))
            {
                cts.Cancel();
                cts.Dispose();
            }
        }

        public bool HasActiveTimer(Guid palyerId)
        {
            return _timers.ContainsKey(palyerId);
        }
    }
}
