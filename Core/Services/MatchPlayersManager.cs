using Core.Interfaces.Services;

namespace Core.Services
{
    public class MatchPlayersManager : IMatchPlayersManager
    {
        private readonly List<Guid> _players;

        public MatchPlayersManager()
        {
            _players = new List<Guid>();
        }

        public void MarkAsDisconnected(Guid playerId)
        {
            var player = _players.FirstOrDefault(p => p == playerId);

            if (player == null)
            {
                _players.Add(playerId);
            }
        }

        public bool IsStillDisconnected(Guid playerId)
        {
            var player = _players.FirstOrDefault(p => p == playerId);

            return player != null;
        }

        public void MarkAsConnected(Guid playerId)
        {
            _players.RemoveAll(p => p == playerId);
        }
    }
}
