using Core.Enums;
using Core.Interfaces.Services;
using Core.Models.Match;

namespace Core.Services
{
    public class MatchTurnService : IMatchTurnService
    {
        private const int FixedTurnMinutes = 1;
        private const int ChessTurnMinutes = 100;
        private readonly IMatchTimer _matchTimer;

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

        public void SetTimeLeft(GameType gameType, PlayerModel player)
        {
            var timeLeft = TimeSpan.Zero;

            if (gameType == GameType.Chess)
            {
                player.Timer.TimeLeft = TimeSpan.FromMinutes(ChessTurnMinutes);
            }
            else
            {
                player.Timer.TimeLeft = TimeSpan.FromMinutes(FixedTurnMinutes);
            }
        }

        //private methods

        private void StartFixedTurn(PlayerModel player)
        {
            player.Timer.TurnExpiresAt = DateTime.UtcNow.AddMinutes(FixedTurnMinutes);
        }

        private void StartChessTurn(PlayerModel player)
        {
            player.Timer.TurnExpiresAt = DateTime.UtcNow.Add(player.Timer.TimeLeft);
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
