using Core.Models.Match;

namespace Core.Interfaces.Repositories
{
    public interface IMatchRepository
    {
        Task<MatchModel> AddMatchAsync(AddMatchModel data);
        Task TryAddPlayerToMatchAsync(Guid playerId, Guid matchId);
        Task<IEnumerable<MatchModel>> GetActiveMatchesAsync(int gameId);
        Task<MatchDetailsModel> GetMatchDetailsAsync(Guid matchId);
        Task<int> GetGameIdAsync(Guid matchId);
    }
}
