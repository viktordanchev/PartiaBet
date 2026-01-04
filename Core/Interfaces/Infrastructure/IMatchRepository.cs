using Core.Enums;
using Core.Models.Match;

namespace Core.Interfaces.Infrastructure
{
    public interface IMatchRepository
    {
        Task<MatchModel> AddMatchAsync(AddMatchModel data);
        Task<PlayerModel> AddPlayerAsync(Guid playerId, Guid matchId);
        Task RemovePlayerAsync(Guid playerId, Guid matchId);
        Task<IEnumerable<MatchModel>> GetActiveMatchesAsync(GameType gameType);
        Task<MatchModel> GetMatchAsync(Guid matchId);
        Task<MatchModel> GetMatchInternalAsync(Guid matchId);
    }
}
