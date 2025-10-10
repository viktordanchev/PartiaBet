using Core.Enums;
using Games.Dtos;

namespace Interfaces.Games
{
    public interface IMatchManagerService
    {
        Guid AddMatch(MatchDto match);
        MatchDto AddPersonToMatch(GameType gameType, Guid matchId, PlayerDto player);
        List<MatchDto> GetMatches(GameType gameType);
    }
}
