using Core.Enums;

namespace Core.Interfaces.Services
{
    public interface IMatchTimer
    {
        double StartTurnTimer(GameType gameType, Guid matchId, Guid playerId);
        double StartLeaverTimer(GameType gameType, Guid matchId, Guid playerId);
        void PauseTurnTimer(GameType gameType, Guid playerId);
        bool HasActiveTimer(Guid playerId);
    }
}
