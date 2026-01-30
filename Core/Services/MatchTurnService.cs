using Core.Enums;
using Core.Interfaces.Services;
using Core.Models.Match;

namespace Core.Services
{
    public class MatchTurnService : IMatchTurnService
    {
        private readonly IMatchTimer _matchTimer;
        private const int FixedTurnSeconds = 60;
        private const int ChessInitialMinutes = 10;

        public MatchTurnService(IMatchTimer matchTimer)
        {
            _matchTimer = matchTimer;
        }

        public void StartTurn(MatchModel match, PlayerModel player)
        {
            if (match.GameType == GameType.Chess)
            {
                StartChessTurn(player);
            }
            else
            {
                StartFixedTurn(player);
            }

            _matchTimer.StartTurnTimer(player);
        }

        public void EndTurn(MatchModel match, PlayerModel player)
        {
            if (match.GameType == GameType.Chess)
            {
                EndChessTurn(player);
            }
            else
            {
                player.Timer.TimeLeft = TimeSpan.Zero;
            }

            _matchTimer.RemoveTimer(player.Id);
        }

        //private methods

        private void StartFixedTurn(PlayerModel player)
        {
            player.Timer = new PlayerTimer
            {
                TurnExpiresAt = DateTime.UtcNow.AddSeconds(FixedTurnSeconds),
                TimeLeft = TimeSpan.Zero
            };
        }

        private void StartChessTurn(PlayerModel player)
        {
            player.Timer ??= new PlayerTimer
            {
                TimeLeft = TimeSpan.FromMinutes(ChessInitialMinutes)
            };

            player.Timer.TurnExpiresAt = DateTime.UtcNow.Add(player.Timer.TimeLeft);
        }

        private void EndChessTurn(PlayerModel player)
        {
            if (player.Timer == null)
                return;

            var remaining = player.Timer.TurnExpiresAt - DateTime.UtcNow;

            player.Timer.TimeLeft = remaining > TimeSpan.Zero
                ? remaining
                : TimeSpan.Zero;
        }
    }
}
