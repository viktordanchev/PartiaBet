using Core.Enums;
using Core.Games.Dtos;
using Core.Games.Models;
using Core.Interfaces.Games;
using System.Collections.Concurrent;
using System.Globalization;

namespace Core.Games.Services
{
    public class MatchManagerService : IMatchManagerService
    {
        private readonly ConcurrentDictionary<GameType, List<Match>> games;

        public MatchManagerService()
        {
            games = new ConcurrentDictionary<GameType, List<Match>>();
        }

        public List<MatchDto> GetMatches(GameType gameType)
        {
            if (games.ContainsKey(gameType))
            {
                return games[gameType]
                    .Select(g => new MatchDto()
                    {
                        Id = g.Id,
                        BetAmount = g.BetAmount,
                        //Players = g.Players.Select(p => new PlayerDto()
                        //{
                        //    Id = p.Id,
                        //    Username = p.Username,
                        //    ProfileImageUrl = p.ProfileImageUrl,
                        //    Rating = p.Rating
                        //})
                        //.ToList()
                    })
                    .ToList();
            }

            return new List<MatchDto>();
        }

        public Guid AddMatch(MatchDto match)
        {
            if (!games.ContainsKey(match.GameType))
            {
                games.TryAdd(match.GameType, new List<Match>());
            }

            var newMatch = new Match()
            {
                BetAmount = match.BetAmount,
                DateAndTime = DateTime.Parse(match.DateAndTime, new CultureInfo("bg-BG")),
                Board = GameBoardFactory.Create(match.GameType),
            };

            games[match.GameType].Add(newMatch);

            return newMatch.Id;
        }

        public MatchDto AddPersonToMatch(GameType gameType, Guid matchId, PlayerDto player)
        {
            var match = games[gameType].FirstOrDefault(m => m.Id == matchId);
            
           //if (match.Players.Count == match.TeamsCount)
           //{
           //
           //}
           //
           //player.Team = match.Players.Count / playersInTeam + 1;
           //
           //match.Players.Add(new Player() 
           //{ 
           //    Id = player.Id, 
           //    Username = player.Username, 
           //    ProfileImageUrl = player.ProfileImageUrl, 
           //    Rating = player.Rating,
           //    Team = player.Team
           //});

            return new MatchDto() 
            {
                Id = match.Id,
                BetAmount = match.BetAmount,
               //layers = match.Players
               //   .Select(p => new PlayerDto()
               //   {
               //       Id = p.Id,
               //       Username = p.Username,
               //       ProfileImageUrl = p.ProfileImageUrl,
               //       Rating = p.Rating,
               //       Team = p.Team
               //   })
               //   .ToList()
            };

        }
    }
}
