using Core.Enums;
using Games.Dtos.MatchManagerService;

namespace Interfaces.Games
{
    public interface IMatchManagerService
    {
        GameType GetGame(Guid matchId);
        MatchDto AddMatch(CreateMatchDto match);
        PlayerDto AddPersonToMatch(Guid matchId, AddPlayerDto player);
        List<MatchDto> GetMatches(GameType gameId);
        MatchDetailsDto GetMatch(Guid matchId);
        void UpdateMatchBoard(Guid matchId, BaseMoveDto move);
    }
}
