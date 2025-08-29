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

            for (int i = 0; i < match.MaxPlayers; i++)
            {
                match.Players.Append(new PlayerDto());
            }

            games[match.GameId].Add(match);
        }
    }
}
