using Core.Interfaces.Services;
using System.Collections.Concurrent;

namespace Core.Services
{
    public class UserConnectionTracker : IUserConnectionTracker
    {
        private readonly ConcurrentDictionary<Guid, int> _connections = new();

        public void AddConnection(Guid userId)
        {
            _connections.AddOrUpdate(userId, 1, (_, count) => count + 1);
        }

        public void RemoveConnection(Guid userId)
        {
            if (!_connections.TryGetValue(userId, out var count))
                return;

            if (count <= 1)
            {
                _connections.TryRemove(userId, out _);
            }
            else
            {
                _connections[userId] = count - 1;
            }
        }

        public bool HasConnections(Guid userId)
        {
            return _connections.ContainsKey(userId);
        }

        public int GetConnectionCount(Guid userId)
        {
            return _connections.TryGetValue(userId, out var count) ? count : 0;
        }
    }
}
