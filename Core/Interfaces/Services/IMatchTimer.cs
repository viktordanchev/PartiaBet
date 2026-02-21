using Core.Enums;
using Core.Models.Match;

namespace Core.Interfaces.Services
{
    public interface IMatchTimer
    {
        void StartTurnTimer(PlayerModel player);
        void PauseTurnTimer(PlayerModel player);
        void StartMatchCountdown(GameType gameType, Guid matchId, TimeSpan countdown);
        void RemoveTimer(Guid key);
    }
}
