using Core.Enums;
using Core.Results.Match;

namespace Core.Interfaces.Services
{
    public interface IMatchHubNotifier
    {
        Task RemoveMatch(GameType gameType, Guid matchId);
        Task EndMatch(EndMatchResult result);
    }
}
