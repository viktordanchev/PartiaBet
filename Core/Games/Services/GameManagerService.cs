using Core.Games.Dtos;
using Core.Interfaces.Games;
using System.Collections.Concurrent;

namespace Core.Games.Services
{
    public class GameManagerService : IGameManagerService
    {
        private readonly ConcurrentDictionary<int, List<MatchDto>> games;

        public GameManagerService()
        {
            games = new ConcurrentDictionary<int, List<MatchDto>>();
        }

        public void CreateMatch(MatchDto match)
        {
            if (!games.ContainsKey(match.GameId))
            {
                games.TryAdd(match.GameId, new List<MatchDto>());
            }

            games[match.GameId].Add(match);
        }

        public void JoinMatch(int gameId, Guid matchId, PlayerDto player)
        {
            var match = games[gameId].FirstOrDefault(m => m.Id == matchId);

            if (games.ContainsKey(gameId) && !match.Players.Any())
            {
                match.Players = new List<PlayerDto>();
            }

            match.Players.Add(player);
        }
    }
}
