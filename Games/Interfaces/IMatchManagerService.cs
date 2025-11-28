using Core.Enums;
using Games.Dtos.Request;
using Games.Dtos.Response;

namespace Interfaces.Games
{
    public interface IMatchManagerService
    {
        MatchResponse AddMatch(CreateMatchRequest match);
        PlayerResponse AddPersonToMatch(Guid matchId, AddPlayerRequest player);
        List<MatchResponse> GetMatches(GameType gameId);
        MatchRoomResponse GetMatch(Guid matchId);
    }
}
