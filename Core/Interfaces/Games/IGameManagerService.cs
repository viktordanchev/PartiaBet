using Core.Games.Dtos;

namespace Core.Interfaces.Games
{
    public interface IGameManagerService
    {
        void CreateMatch(MatchDto match);
        void JoinMatch(int gameId, Guid matchId, PlayerDto player);
    }
}
