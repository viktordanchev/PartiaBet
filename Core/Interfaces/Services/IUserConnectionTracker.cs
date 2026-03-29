namespace Core.Interfaces.Services
{
    public interface IUserConnectionTracker
    {
        void AddConnection(Guid userId, string connectionId);
        void RemoveConnection(Guid userId, string connectionId);
        bool HasConnections(Guid userId);
        int GetConnectionCount(Guid userId);
    }
}
