using Core.Games.Dtos;
using Core.Interfaces.Games;
using System.Collections.Concurrent;

namespace Core.Games.Services
{
    public class GameManagerService : IGameManagerService
    {
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, List<User>>> games;

        public GameManagerService()
        {
            games = new ConcurrentDictionary<string, ConcurrentDictionary<string, List<User>>>();
        }

        public void AddMatchToGame(string game, string matchId)
        {
            if (!games.ContainsKey(game))
            {
                games.TryAdd(game, new ConcurrentDictionary<string, List<User>>());
            }

            games[game].TryAdd(matchId, new List<User>());
        }

        public void AddUserToMatch(string game, string matchId, User user)
        {
            if (!games[game].ContainsKey(matchId))
            {
                games[game].TryAdd(matchId, new List<User>());
            }

            games[game][matchId].Add(user);
        }
    }
}
