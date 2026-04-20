using Core.Enums;
using Microsoft.AspNetCore.SignalR;
using RestAPI.Dtos.Match;

namespace RestAPI.Services.Interfaces
{
    public interface IMatchRealtimeService
    {
        Task OnConnected(HubCallerContext context);
        Task OnDisconnected(HubCallerContext context, Exception? ex);

        Task JoinGameGroup(string connectionId, GameType gameType);
        Task JoinMatchGroup(string connectionId, Guid matchId);

        Task<Guid> CreateMatch(HubCallerContext context, AddMatchDto data);
        Task JoinMatch(HubCallerContext context, Guid matchId);

        Task MakeMove(HubCallerContext context, Guid matchId, string jsonData);

        Task LeaveMatchQueue(HubCallerContext context);
        Task RejoinMatch(HubCallerContext context);
    }
}
