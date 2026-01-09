using Core.Enums;
using Core.Models.Games;
using Core.Models.Match;

namespace Core.Interfaces.Services
{
    public interface IMatchService
    {
        Task<MatchModel> GetMatchInternalAsync(Guid matchId);
        Task<MatchModel> CreateMatchAsync(AddMatchModel data);
        Task<PlayerModel> AddPlayerAsync(MatchModel match, Guid playerId);
        Task<Guid> UpdatePlayerStatusAsync(Guid playerId, PlayerStatus status);
        Task RemovePlayerAsync(MatchModel match, Guid playerId);
        Task<IEnumerable<MatchModel>> GetActiveMatchesAsync(GameType gameType);
        Task<MatchModel> GetMatchAsync(Guid matchId);
        Task<MoveResultModel> TryMakeMoveAsync(MatchModel match, Guid playerId, string moveJson);
        Task EndMatchAsync(Guid matchId);
        Task<Guid> SwtichTurnAsync(MatchModel match, Guid currentPlayerId);
    }
}
