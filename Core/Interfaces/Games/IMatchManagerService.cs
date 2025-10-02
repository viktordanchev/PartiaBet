using Core.Enums;
using Core.Games.Dtos;

namespace Core.Interfaces.Games
{
    public interface IMatchManagerService
    {
        Guid AddMatch(MatchDto match);
        MatchDto AddPersonToMatch(GameType gameType, Guid matchId, PlayerDto player);
        List<MatchDto> GetMatches(GameType gameType);
    }
}
