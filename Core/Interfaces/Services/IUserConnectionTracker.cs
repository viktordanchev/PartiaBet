namespace Core.Interfaces.Services
{
    public interface IUserConnectionTracker
    {
        void AddConnection(Guid userId, string hubName, string connectionId);
        void RemoveConnection(Guid userId, string hubName, string connectionId);
        int GetConnectionCount(Guid userId, string hubName);
        bool HasConnections(Guid userId, string hubName);
        bool HasAnyConnections(Guid userId);
    }
}
