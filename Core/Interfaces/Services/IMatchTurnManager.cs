using Core.Enums;
using Core.Models.Match;

namespace Core.Interfaces.Services
{
    public interface IMatchTurnManager
    {
        public void StartTurn(MatchModel match, PlayerModel player);
        public void StartDisconnectCountdown(Guid matchId);
        void PauseTurn(PlayerModel player);
        public void EndTurn(GameType gameType, PlayerModel player);
        public void RemoveTimer(Guid key);
    }
}
