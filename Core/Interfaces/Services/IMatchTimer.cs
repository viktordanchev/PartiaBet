using Core.Enums;

namespace Core.Interfaces.Services
{
    public interface IMatchTimer
    {
        double StartTurnTimer(GameType gameType, Guid matchId, Guid playerId);
        double StartMatchCountdown(GameType gameType, Guid matchId);
        void PauseTurnTimer(GameType gameType, Guid playerId);
        double GetActiveTimer(Guid key);
        void RemoveTimer(Guid matchId);
    }
}
