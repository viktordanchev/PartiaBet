using Core.Games.Dtos;
using System.Collections.Concurrent;

namespace Core.Games.Services
{
    public class GameManagerService
    {
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, List<User>>> games;

        public GameManagerService()
        {
            games = new ConcurrentDictionary<string, ConcurrentDictionary<string, List<User>>>();
        }

        public void RegisterGame(string game, string gameId)
        {
            
        }

        public void AddMatchToGame(string game, string matchId)
        {
            if (!games.ContainsKey(game))
            {
                games.TryAdd(game, new ConcurrentDictionary<string, List<User>>());
            }

            games[game].TryAdd(matchId, new List<User>());
        }

        public void AddUserToGame(string gameId, User player)
        {
            if (games.ContainsKey(gameId))
            {
                games[gameId].Add(player);
            }
        }
    }
}
