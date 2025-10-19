using Core.Enums;
using Games.Dtos.Request;
using Games.Dtos.Response;
using Games.Models;
using Interfaces.Games;
using System.Collections.Concurrent;
using System.Globalization;

namespace Games.Services
{
    public class MatchManagerService : IMatchManagerService
    {
        private readonly ConcurrentDictionary<GameType, ConcurrentDictionary<Guid, MatchModel>> games;

        public MatchManagerService()
        {
            games = new ConcurrentDictionary<GameType, ConcurrentDictionary<Guid, MatchModel>>();
        }

        public List<MatchResponse> GetMatches(GameType game)
        {
            if (!games.ContainsKey(game))
            {
                return new List<MatchResponse>();
            }

            return games[game]
                    .Select(m => new MatchResponse()
                    {
                        Id = m.Key,
                        BetAmount = m.Value.BetAmount,
                        MaxPlayersCount = m.Value.MaxPlayersCount,
                        Players = m.Value.Players
                            .Select(p => new PlayerResponse()
                            {
                                Id = p.Id,
                                Username = p.Username,
                                ProfileImageUrl = p.ProfileImageUrl,
                                Rating = p.Rating
                            }).ToList()
                    })
                    .ToList();
        }

        public MatchResponse AddMatch(CreateMatchRequest match)
        {
            if (!games.ContainsKey(match.GameId) && Enum.IsDefined(typeof(GameType), match.GameId))
            {
                games.TryAdd(match.GameId, new ConcurrentDictionary<Guid, MatchModel>());
            }

            var gameConfigs = GameFactory.GetGameConfigs(match.GameId);
            var newMatch = new MatchModel()
            {
                BetAmount = match.BetAmount,
                DateAndTime = DateTime.Parse(match.DateAndTime, new CultureInfo("bg-BG")),
                MaxPlayersCount = gameConfigs.TeamSize * gameConfigs.TeamsCount
            };

            var newMatchId = Guid.NewGuid();
            games[match.GameId].TryAdd(newMatchId, newMatch);

            return new MatchResponse()
            {
                Id = newMatchId,
                GameId = match.GameId,
                BetAmount = newMatch.BetAmount,
                MaxPlayersCount = newMatch.MaxPlayersCount,
            };
        }

        public PlayerResponse AddPersonToMatch(GameType game, Guid matchId, AddPlayerRequest player)
        {
            var match = games[game][matchId];
            var gameConfigs = GameFactory.GetGameConfigs(game);

            if (match!.Players.Count == gameConfigs.TeamsCount * gameConfigs.TeamSize)
            {
                
            }

            match.Players.Add(new PlayerModel()
            {
                Id = player.Id,
                Username = player.Username,
                ProfileImageUrl = player.ProfileImageUrl,
                Rating = 1000,
                Team = match.Players.Count / gameConfigs.TeamSize + 1,
            });

            return new PlayerResponse()
            {
                Id = player.Id,
                Username = player.Username,
                ProfileImageUrl = player.ProfileImageUrl,
                Rating = 1000
            };

        }

        public decimal GetMatch(GameType game, Guid matchId)
        {
            return games[game][matchId].BetAmount;
        }
    }
}
