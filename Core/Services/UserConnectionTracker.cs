using Core.Interfaces.Services;
using System.Collections.Concurrent;

namespace Core.Services
{
    public class UserConnectionTracker : IUserConnectionTracker
    {
        private readonly ConcurrentDictionary<Guid, HashSet<string>> _connections = new();

        public void AddConnection(Guid userId, string connectionId)
        {
            var connections = _connections.GetOrAdd(userId, _ => new HashSet<string>());

            lock (connections)
            {
                connections.Add(connectionId);
            }
        }

        public void RemoveConnection(Guid userId, string connectionId)
        {
            if (!_connections.TryGetValue(userId, out var connections))
                return;

            lock (connections)
            {
                connections.Remove(connectionId);

                if (connections.Count == 0)
                {
                    _connections.TryRemove(userId, out _);
                }
            }
        }

        public bool HasConnections(Guid userId)
        {
            return _connections.ContainsKey(userId);
        }

        public int GetConnectionCount(Guid userId)
        {
            return _connections.TryGetValue(userId, out var connections)
                ? connections.Count
                : 0;
        }
    }
}
