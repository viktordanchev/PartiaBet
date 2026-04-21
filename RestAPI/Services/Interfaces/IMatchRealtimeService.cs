using Core.Enums;
using RestAPI.Dtos.Match;

namespace RestAPI.Services.Interfaces
{
    public interface IMatchRealtimeService
    {
        Task OnConnectedAsync(Guid userId, string connectionId);

        Task OnDisconnectedAsync(Guid userId, string connectionId);

        Task JoinGameGroupAsync(string connectionId, GameType gameType);

        Task JoinMatchGroupAsync(string connectionId, Guid matchId);

        Task<Guid> CreateMatchAsync(Guid userId, string connectionId, AddMatchDto data);

        Task JoinMatchAsync(Guid userId, string connectionId, Guid matchId);

        Task MakeMoveAsync(Guid userId, Guid matchId, string jsonData);

        Task LeaveMatchQueueAsync(Guid userId);

        Task RejoinMatchAsync(Guid userId, string connectionId);
    }
}
