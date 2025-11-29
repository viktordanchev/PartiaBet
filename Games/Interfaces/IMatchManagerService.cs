using Core.Enums;
using Games.Dtos.Request;
using Games.Dtos.Response;

namespace Interfaces.Games
{
    public interface IMatchManagerService
    {
        GameType GetGame(Guid matchId);
        MatchResponse AddMatch(CreateMatchRequestDto match);
        PlayerResponse AddPersonToMatch(Guid matchId, AddPlayerRequestDto player);
        List<MatchResponse> GetMatches(GameType gameId);
        MatchRoomResponse GetMatch(Guid matchId);
    }
}
