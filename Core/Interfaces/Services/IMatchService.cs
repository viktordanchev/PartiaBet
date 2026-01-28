using Core.Enums;
using Core.Models.Match;
using Core.Results.Match;

namespace Core.Interfaces.Services
{
    public interface IMatchService
    {
        Task<MatchModel> CreateMatchAsync(GameType gameType, decimal betAmount);
        Task<JoinMatchResult> JoinMatchAsync(Guid matchId, Guid playerId);
        Task<LeaveMatchResult> LeaveMatchAsync(Guid matchId, Guid playerId);
        Task<МакеMoveResult> MakeMoveAsync(Guid matchId, Guid playerId, string moveDataJson);
        Task<IEnumerable<MatchModel>> GetActiveMatchesAsync(GameType gameType);
        Task<MatchModel> GetMatchAsync(Guid matchId);
        Task<RejoinMatchResult> RejoinMatchAsync(Guid matchId, Guid playerId);
        Task<double> GetMatchAutoEndTimeRemainingAsync(Guid playerId);
    }
}
