using Core.Models.Match;

namespace Core.Interfaces.Infrastructure
{
    public interface IMatchRepository
    {
        Task<MatchModel> AddMatchAsync(AddMatchModel data);
        Task<PlayerModel> TryAddPlayerToMatchAsync(Guid playerId, Guid matchId);
        Task TryRemovePlayerFromMatchAsync(Guid playerId, Guid matchId);
        Task<IEnumerable<MatchModel>> GetActiveMatchesAsync(int gameId);
        Task<MatchDetailsModel> GetMatchDetailsAsync(Guid matchId);
        Task<int> GetGameIdAsync(Guid matchId);
        Task<int> GetPlayersCountAsync(Guid matchId);
    }
}
