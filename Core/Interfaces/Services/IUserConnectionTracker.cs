namespace Core.Interfaces.Services
{
    public interface IUserConnectionTracker
    {
        void AddConnection(Guid userId);
        void RemoveConnection(Guid userId);
        bool HasConnections(Guid userId);
        int GetConnectionCount(Guid userId);
    }
}
