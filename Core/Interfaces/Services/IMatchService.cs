using Core.Enums;
using Core.Models.Match;
using Core.Results.Match;

namespace Core.Interfaces.Services
{
    public interface IMatchService
    {
        Task<MatchModel> CreateMatchAsync(GameType gameType, decimal betAmount);
        Task<JoinMatchResult> JoinMatchAsync(Guid matchId, Guid playerId);
        Task<LeaveMatchQueueResult> LeaveMatchQueueAsync(Guid playerId);
        Task<МакеMoveResult> MakeMoveAsync(Guid matchId, Guid playerId, string moveDataJson);
        Task<IEnumerable<MatchModel>> GetActiveMatchesAsync(GameType gameType);
        Task<MatchModel> GetMatchAsync(Guid matchId);
        Task<PlayerRejoinMatchResult> PlayerRejoinMatchAsync(Guid playerId);
        Task<HandlePlayerDisconnectResult> GetMatchCountdownAsync(Guid playerId);
        Task EndMatchAsync(Guid matchId);
        Task<HandlePlayerDisconnectResult> HandlePlayerDisconnectAsync(Guid playerId);
    }
}
