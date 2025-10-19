using Common.Exceptions;
using Core.Enums;
using Games.Dtos.Request;
using Games.Dtos.Response;
using Games.Models;
using Interfaces.Games;
using System.Collections.Concurrent;
using System.Globalization;
using static Common.Constants.ErrorMessages;

namespace Games.Services
{
    public class MatchManagerService : IMatchManagerService
    {
        private readonly ConcurrentDictionary<GameType, List<MatchModel>> games;

        public MatchManagerService()
        {
            games = new ConcurrentDictionary<GameType, List<MatchModel>>();
        }

        public List<MatchResponse> GetMatches(GameType gameId)
        {
            if (!games.ContainsKey(gameId))
            {
                return new List<MatchResponse>();
            }

            return games[gameId]
                    .Select(m => new MatchResponse()
                    {
                        Id = m.Id,
                        BetAmount = m.BetAmount,
                        MaxPlayersCount = m.MaxPlayersCount,
                        Players = m.Players
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
                games.TryAdd(match.GameId, new List<MatchModel>());
            }

            var gameConfigs = GameFactory.GetGameConfigs(match.GameId);
            var newMatch = new MatchModel()
            {
                BetAmount = match.BetAmount,
                DateAndTime = DateTime.Parse(match.DateAndTime, new CultureInfo("bg-BG")),
                MaxPlayersCount = gameConfigs.TeamSize * gameConfigs.TeamsCount
            };

            games[match.GameId].Add(newMatch);

            return new MatchResponse()
            {
                Id = newMatch.Id,
                GameId = match.GameId,
                BetAmount = newMatch.BetAmount,
                MaxPlayersCount = newMatch.MaxPlayersCount,
            };
        }

        public PlayerResponse AddPersonToMatch(GameType gameId, Guid matchId, AddPlayerRequest player)
        {
            var match = games[gameId].FirstOrDefault(m => m.Id == matchId);
            var gameConfigs = GameFactory.GetGameConfigs(gameId);

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

        public void IsGameAndMatchExist(GameType gameId, Guid matchId)
        {
            if (!games.ContainsKey(gameId) || !games[gameId].Any(m => m.Id == matchId))
            {
                throw new ApiException(InvalidRequest);
            }
        }
    }
}
