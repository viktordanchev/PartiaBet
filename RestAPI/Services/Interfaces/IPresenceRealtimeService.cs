namespace RestAPI.Services.Interfaces
{
    public interface IPresenceRealtimeService
    {
        Task OnConnectedAsync(Guid userId, string connectionId);
        Task OnDisconnectedAsync(Guid userId, string connectionId);
    }
}
