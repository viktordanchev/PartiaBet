using Core.Enums;
using Core.Models.Match;

namespace Core.Interfaces.Infrastructure
{
    public interface ICacheService
    {
        Task SetMatchAsync(Guid matchId, MatchModel match);
        Task<MatchModel> GetMatchAsync(Guid matchId);
        Task<IEnumerable<MatchModel>> GetAllMatchesAsync(GameType gameType);
    }
}
