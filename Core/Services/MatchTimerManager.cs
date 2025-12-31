using System.Collections.Concurrent;

namespace Core.Services
{
    public class MatchTimerManager
    {
        private readonly ConcurrentDictionary<Guid, CancellationTokenSource> _timers
            = new ConcurrentDictionary<Guid, CancellationTokenSource>();

        public void StartTimer(Guid matchId, TimeSpan duration, Func<Guid, Task> onTimeout)
        {
            StopTimer(matchId);

            var cts = new CancellationTokenSource();
            _timers[matchId] = cts;

            _ = Task.Run(async () =>
            {
                try
                {
                    await Task.Delay(duration, cts.Token);

                    if (!cts.Token.IsCancellationRequested)
                    {
                        await onTimeout(matchId);
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

        public bool HasActiveTimer(Guid matchId)
        {
            return _timers.ContainsKey(matchId);
        }
    }
}
