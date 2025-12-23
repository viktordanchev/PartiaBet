using Core.Enums;
using Core.Models.Match;

namespace Core.Interfaces.Services
{
    public interface IMatchService
    {
        Task<MatchModel> CreateMatchAsync(AddMatchModel data);
        Task AddPersonToMatch(Guid matchId, Guid playerId);
        Task<IEnumerable<MatchModel>> GetActiveMatchesAsync(int gameId);
        Task<MatchDetailsModel> GetMatch(Guid matchId);
        Task<GameType> GetMatchGameTypeAsync(Guid matchId);
    }
}
