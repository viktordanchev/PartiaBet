using Core.Enums;
using Core.Interfaces.Services;
using Core.Models.Match;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using static Common.Constants.Constants;

namespace Core.Services
{
    public class MatchTurnManager : IMatchTurnManager
    {
        private readonly ConcurrentDictionary<Guid, CancellationTokenSource> _timers;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private const int FixedTurnMinutes = 1;

        public MatchTurnManager(IServiceScopeFactory serviceScopeFactory)
        {
            _timers = new();
            _serviceScopeFactory = serviceScopeFactory;
        }

        public void StartTurn(MatchModel match, PlayerModel player)
        {
            if (match.GameType == GameType.Chess)
            {
                player.Timer.TurnExpiresAt = DateTime.UtcNow.Add(player.Timer.TimeLeft);
            }
            else
            {
                player.Timer.TimeLeft = TimeSpan.FromMinutes(FixedTurnMinutes);
                player.Timer.TurnExpiresAt = DateTime.UtcNow.AddMinutes(FixedTurnMinutes);
            }

            player.Timer.IsPaused = false;

            StartTurnTimer(match, player);
        }
        
        public void StartDisconnectCountdown(Guid matchId)
        {
            var cts = new CancellationTokenSource();
            _timers[matchId] = cts;

            _ = Task.Run(async () =>
            {
                try
                {
                    var countdown = TimeSpan.FromSeconds(MatchEndCountdownSeconds);
                    await Task.Delay(countdown, cts.Token);
                    _timers.TryRemove(matchId, out _);

                    using var scope = _serviceScopeFactory.CreateScope();
                    var matchService = scope.ServiceProvider.GetRequiredService<IMatchService>();
                    var matchHubNotifier = scope.ServiceProvider.GetRequiredService<IMatchHubNotifier>();

                    await matchService.SetMatchDrawAsync(matchId);
                    var result = await matchService.EndMatchAsync(matchId);

                    await matchHubNotifier.EndMatch(result);
                }
                catch (TaskCanceledException) { }
            });
        }

        public void PauseTurn(PlayerModel player)
        {
            if (_timers.TryRemove(player.Id, out var cts))
            {
                cts.Cancel();
                cts.Dispose();
            }

            player.Timer.IsPaused = true;
        }

        public void EndTurn(GameType gameType, PlayerModel player)
        {
            PauseTurn(player);

            if (gameType == GameType.Chess)
                EndChessTurn(player);
            else
                player.Timer.TimeLeft = TimeSpan.Zero;

            player.Timer.IsPaused = true;
        }

        public void RemoveTimer(Guid key)
        {
            if (_timers.TryRemove(key, out var cts))
            {
                cts.Cancel();
                cts.Dispose();
            }
        }

        //private methods

        private void StartTurnTimer(MatchModel match, PlayerModel player)
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
                    var matchHubNotifier = scope.ServiceProvider.GetRequiredService<IMatchHubNotifier>();

                    if (match.GameType == GameType.Chess)
                    {
                        await matchService.SetMatchDrawAsync(match.Id);
                        var result = await matchService.EndMatchAsync(match.Id);

                        await matchHubNotifier.EndMatch(result);
                    }
                    else
                    {
                        matchService.SwitchTurnAsync(match, player.Id);
                    }
                }
                catch (TaskCanceledException) { }
            });
        }

        private void EndChessTurn(PlayerModel player)
        {
            if (player.Timer.TimeLeft == TimeSpan.Zero)
                return;

            var remaining = player.Timer.TurnExpiresAt - DateTime.UtcNow;

            player.Timer.TimeLeft = remaining > TimeSpan.Zero
                ? remaining
                : TimeSpan.Zero;
        }
    }
}
