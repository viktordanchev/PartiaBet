using Core.Enums;
using Core.Models.Match;
using Core.Results.Match;

namespace Core.Interfaces.Services
{
    public interface IMatchService
    {
        Task<MatchModel> CreateMatchAsync(GameType gameType, decimal betAmount);
        Task<AddPlayerResult> AddPlayerAsync(Guid matchId, Guid playerId);
        Task<LeaveMatchResult> LeaveMatchAsync(Guid matchId, Guid playerId);
        Task<МакеMoveResult> UpdateBoardAsync(Guid matchId, Guid playerId, string moveDataJson); 
        Task<Guid> SwtichTurnAsync(Guid matchId, Guid currentPlayerId);
        Task<IEnumerable<MatchModel>> GetActiveMatchesAsync(GameType gameType);
        Task<MatchModel> GetMatchAsync(Guid matchId);
        Task<RejoinMatchResult> RejoinMatchAsync(Guid matchId, Guid playerId);
        Task<double> GetPlayerRejoinTimeAsync(Guid playerId);
    }
}
