using Core.Enums;
using Games.Chess;
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
        private readonly ConcurrentDictionary<Guid, MatchModel> matches;

        public MatchManagerService()
        {
            matches = new ConcurrentDictionary<Guid, MatchModel>();
        }

        public GameType GetGame(Guid matchId)
        {
            return matches[matchId].Game;
        }

        public List<MatchResponse> GetMatches(GameType game)
        {
            return matches
                .Where(m => m.Value.Game == game)
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
            var gameConfigs = GameFactory.GetGameConfigs(match.GameId);
            var newMatch = new MatchModel()
            {
                Game = match.GameId,
                BetAmount = match.BetAmount,
                DateAndTime = DateTime.Parse(match.DateAndTime, new CultureInfo("bg-BG")),
                MaxPlayersCount = gameConfigs.TeamSize * gameConfigs.TeamsCount,
                Board = GameFactory.GetGameBoard(match.GameId)
            };
            
            var newMatchId = Guid.NewGuid();

            matches.TryAdd(newMatchId, newMatch);

            return new MatchResponse()
            {
                Id = newMatchId,
                GameId = match.GameId,
                BetAmount = newMatch.BetAmount,
                MaxPlayersCount = newMatch.MaxPlayersCount,
            };
        }

        public PlayerResponse AddPersonToMatch(Guid matchId, AddPlayerRequest player)
        {
            var match = matches[matchId];

            var gamei = match.Board as ChessBoard;
            gamei.AddToBoard(player.Id);

            var gameConfigs = GameFactory.GetGameConfigs(match.Game);

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

        public MatchRoomResponse GetMatch(Guid matchId)
        {
            var match = matches[matchId];

            return new MatchRoomResponse()
            {
                Game = match.Game,
                BetAmount = match.BetAmount,
                SpectatorsCount = match.SpectatorsCount,
                Players = match.Players
                    .Select(p => new PlayerResponse()
                    {
                        Id = p.Id,
                        Username = p.Username,
                        ProfileImageUrl = p.ProfileImageUrl,
                        Rating = p.Rating
                    }).ToList(),
                Board = match.Board
            };
        }
    }
}
