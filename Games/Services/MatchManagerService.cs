using Core.Enums;
using Games.Dtos.MatchManagerService;
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

        public List<MatchDto> GetMatches(GameType game)
        {
            return matches
                .Where(m => m.Value.Game == game)
                .Select(m => new MatchDto()
                {
                    Id = m.Key,
                    BetAmount = m.Value.BetAmount,
                    MaxPlayersCount = m.Value.MaxPlayersCount,
                    Players = m.Value.Players
                        .Select(p => new PlayerDto()
                        {
                            Id = p.Id,
                            Username = p.Username,
                            ProfileImageUrl = p.ProfileImageUrl,
                            Rating = p.Rating
                        }).ToList()
                })
                .ToList();
        }

        public MatchDto AddMatch(CreateMatchDto match)
        {
            var gameService = GameFactory.GetGameService(match.Game);

            var newMatch = new MatchModel()
            {
                Game = match.Game,
                BetAmount = match.BetAmount,
                DateAndTime = DateTime.Parse(match.DateAndTime, new CultureInfo("bg-BG")),
                MaxPlayersCount = gameService.Configs.TeamSize * gameService.Configs.TeamsCount,
                Board = gameService.CreateGameBoard()
            };

            var newMatchId = Guid.NewGuid();

            matches.TryAdd(newMatchId, newMatch);

            return new MatchDto()
            {
                Id = newMatchId,
                Game = match.Game,
                BetAmount = newMatch.BetAmount,
                MaxPlayersCount = newMatch.MaxPlayersCount,
            };
        }

        public PlayerDto AddPersonToMatch(Guid matchId, AddPlayerDto player)
        {
            var match = matches[matchId];
            var gameService = GameFactory.GetGameService(match.Game);

            if (match!.Players.Count == gameService.Configs.TeamsCount * gameService.Configs.TeamSize)
            {

            }

            match.Players.Add(new PlayerModel()
            {
                Id = player.Id,
                Username = player.Username,
                ProfileImageUrl = player.ProfileImageUrl,
                Rating = 1000,
                Team = match.Players.Count / gameService.Configs.TeamSize + 1,
            });

            gameService.AddToBoard(player.Id, match.Board);

            return new PlayerDto()
            {
                Id = player.Id,
                Username = player.Username,
                ProfileImageUrl = player.ProfileImageUrl,
                Rating = 1000
            };

        }

        public MatchDetailsDto GetMatch(Guid matchId)
        {
            var match = matches[matchId];

            return new MatchDetailsDto()
            {
                Game = match.Game,
                BetAmount = match.BetAmount,
                SpectatorsCount = match.SpectatorsCount,
                Players = match.Players
                    .Select(p => new PlayerDto()
                    {
                        Id = p.Id,
                        Username = p.Username,
                        ProfileImageUrl = p.ProfileImageUrl,
                        Rating = p.Rating
                    }).ToList(),
                Board = match.Board
            };
        }

        public void UpdateMatchBoard(Guid matchId, BaseMoveDto move, string playerId)
        {
            var match = matches[matchId];
            var gameService = GameFactory.GetGameService(match.Game);
            gameService.UpdateBoard(match.Board, move);
        }

        public bool IsValidMove(Guid matchId, BaseMoveDto move, string playerId)
        {
            var match = matches[matchId];
            var gameService = GameFactory.GetGameService(match.Game);

            return gameService.IsValidMove(match.Board, move);
        }
    }
}
