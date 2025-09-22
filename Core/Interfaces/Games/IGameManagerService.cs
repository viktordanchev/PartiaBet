using Core.Games.Dtos;

namespace Core.Interfaces.Games
{
    public interface IGameManagerService
    {
        void CreateMatch(MatchDto match);
        MatchDto JoinMatch(int gameId, Guid matchId, PlayerDto player);
        List<MatchDto> GetMatches(int gameId);
    }
}
