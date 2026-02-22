using Core.Interfaces.Services;
using System.Collections.Concurrent;

namespace Core.Services
{
    public class MatchPlayersManager : IMatchPlayersManager
    {
        private readonly ConcurrentDictionary<Guid, byte> _users;

        public MatchPlayersManager()
        {
            _users = new ConcurrentDictionary<Guid, byte>();
        }

        public void MarkAsDisconnected(Guid userId)
        {
            _users.TryAdd(userId, 0);
        }

        public bool IsStillDisconnected(Guid userId)
        {
            return _users.ContainsKey(userId);
        }

        public void MarkAsConnected(Guid userId)
        {
            _users.TryRemove(userId, out _);
        }
    }
}
