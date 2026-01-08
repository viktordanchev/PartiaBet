using Core.Enums;

namespace Core.Interfaces.Services
{
    public interface IMatchTimer
    {
        double StartTurnTimer(GameType gameType, Guid matchId, Guid palyerId);
        void StartLeaverTimer(GameType gameType, Guid matchId, Guid palyerId);
        void PauseTurnTimer(GameType gameType, Guid playerId);
    }
}
