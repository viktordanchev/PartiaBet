using Core.Enums;
using Core.Models.Match;

namespace Core.Interfaces.Services
{
    public interface IMatchService
    {
        Task<MatchModel> CreateMatchAsync(GameType gameType, decimal betAmount);
        Task<MatchStatusModel> AddPlayerAsync(Guid matchId, Guid playerId);
        Task RemovePlayerAsync(Guid matchId, Guid playerId);
        Task<MoveResultModel> UpdateBoardAsync(Guid matchId, Guid playerId, string moveDataJson); 
        Task<Guid> SwtichTurnAsync(Guid matchId, Guid currentPlayerId);
        Task<IEnumerable<MatchModel>> GetActiveMatchesAsync(GameType gameType);
        Task<MatchModel> GetMatchAsync(Guid matchId);
    }
}
