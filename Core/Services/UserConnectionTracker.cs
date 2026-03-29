using Core.Interfaces.Services;
using System.Collections.Concurrent;

namespace Core.Services
{
    public class UserConnectionTracker : IUserConnectionTracker
    {
        private readonly ConcurrentDictionary<Guid, ConcurrentDictionary<string, HashSet<string>>> _connections = new();

        public void AddConnection(Guid userId, string hubName, string connectionId)
        {
            var userHubs = _connections.GetOrAdd(userId, _ => new ConcurrentDictionary<string, HashSet<string>>());

            var connections = userHubs.GetOrAdd(hubName, _ => new HashSet<string>());

            lock (connections)
            {
                connections.Add(connectionId);
            }
        }

        public void RemoveConnection(Guid userId, string hubName, string connectionId)
        {
            if (!_connections.TryGetValue(userId, out var userHubs))
                return;

            if (!userHubs.TryGetValue(hubName, out var connections))
                return;

            lock (connections)
            {
                connections.Remove(connectionId);

                if (connections.Count == 0)
                {
                    userHubs.TryRemove(hubName, out _);
                }
            }

            if (userHubs.IsEmpty)
            {
                _connections.TryRemove(userId, out _);
            }
        }

        public int GetConnectionCount(Guid userId, string hubName)
        {
            if (_connections.TryGetValue(userId, out var userHubs) &&
                userHubs.TryGetValue(hubName, out var connections))
            {
                return connections.Count;
            }

            return 0;
        }

        public bool HasConnections(Guid userId, string hubName)
        {
            return GetConnectionCount(userId, hubName) > 0;
        }

        public bool HasAnyConnections(Guid userId)
        {
            return _connections.ContainsKey(userId);
        }
    }
}
