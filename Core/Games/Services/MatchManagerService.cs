using Core.Enums;
using Core.Games.Dtos;
using Core.Games.Models;
using Core.Interfaces.Games;
using System.Collections.Concurrent;
using System.Globalization;
using static Core.Games.GameConfigs;

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
                    .Select(m => new MatchDto()
                    {
                        Id = m.Id,
                        GameType = gameType,
                        BetAmount = m.BetAmount,
                        Teams = m.Teams
                            .Select(t => new TeamDto()
                            {
                                Id = t.Id,
                                Players = t.Players
                                    .Select(p => new PlayerDto()
                                    {
                                        Id = p.Id,
                                        Username = p.Username,
                                        ProfileImageUrl = p.ProfileImageUrl,
                                        Rating = p.Rating
                                    }).ToList()
                            })
                            .ToList(),
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
            };

            games[match.GameType].Add(newMatch);

            return newMatch.Id;
        }

        public MatchDto AddPersonToMatch(GameType gameType, Guid matchId, PlayerDto player)
        {
            var match = games[gameType].FirstOrDefault(m => m.Id == matchId);
            Team team = null;

            if (match!.Teams.Sum(t => t.Players.Count) == ChessConfigs.TeamsCount * ChessConfigs.TeamSize)
            {

            }
            else if (match!.Teams.All(t => t.Players.Count == ChessConfigs.TeamSize))
            {
                team = new Team();
                match.Teams.Add(team);
            }

            team!.Players.Add(new Player()
            {
                Id = player.Id,
                Username = player.Username,
                ProfileImageUrl = player.ProfileImageUrl,
                Rating = player.Rating,
            });

            return new MatchDto()
            {
                Id = match.Id,
                GameType = gameType,
                BetAmount = match.BetAmount,
                TeamsCount = ChessConfigs.TeamsCount,
                TeamSize = ChessConfigs.TeamSize,
                Teams = match.Teams
                    .Select(t => new TeamDto()
                    {
                        Id = t.Id,
                        Players = t.Players
                            .Select(p => new PlayerDto()
                            {
                                Id = p.Id,
                                Username = p.Username,
                                ProfileImageUrl = p.ProfileImageUrl,
                                Rating = p.Rating
                            }).ToList()
                    })
                    .ToList(),
            };

        }
    }
}
