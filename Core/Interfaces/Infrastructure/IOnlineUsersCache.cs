namespace Core.Interfaces.Infrastructure
{
    public interface IOnlineUsersCache
    {
        Task<IEnumerable<Guid>> GetOnlineUsersAsync(IEnumerable<Guid> userIds);
        Task SetUserOfflineAsync(Guid userId);
        Task SetUserOnlineAsync(Guid userId);
    }
}
