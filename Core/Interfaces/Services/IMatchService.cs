using Core.Enums;
using Core.Models.Games;
using Core.Models.Match;

namespace Core.Interfaces.Services
{
    public interface IMatchService
    {
        Task<MatchModel> CreateMatchAsync(AddMatchModel data);
        Task<PlayerModel> AddPlayerAsync(Guid matchId, Guid playerId);
        Task<bool> RemovePlayerAsync(Guid matchId, Guid playerId);
        Task<IEnumerable<MatchModel>> GetActiveMatchesAsync(GameType gameType);
        Task<MatchModel> GetMatchAsync(Guid matchId);
        Task<(bool, BaseMoveModel)> TryMakeMoveAsync(Guid matchId, Guid playerId, string moveJson);
    }
}
