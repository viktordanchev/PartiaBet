using Common.Exceptions;
using Core.Enums;
using Games.Dtos;
using Games.Models;
using Interfaces.Games;
using System.Collections.Concurrent;
using System.Globalization;
using static Common.Constants.ErrorMessages;

namespace Games.Services
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
            if (!games.ContainsKey(match.GameType) && Enum.IsDefined(typeof(GameType), match.GameType))
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

            IsMatchFull(match!, gameType);

            var gameConfigs = GameFactory.GetGameConfigs(gameType);
            Team team = null;
            
            if (match!.Teams.All(t => t.Players.Count == gameConfigs.TeamSize))
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
                TeamsCount = TeamsCount,
                TeamSize = TeamSize,
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

        public void IsGameAndMatchExist(GameType gameType, Guid matchId)
        {
            if (!games.ContainsKey(gameType) || !games[gameType].Any(m => m.Id == matchId))
            {
                throw new ApiException(InvalidRequest);
            }
        }

        private void IsMatchFull(Match match, GameType gameType)
        {
            var gameConfigs = GameFactory.GetGameConfigs(gameType);

            if(match.Teams.Sum(t => t.Players.Count) == gameConfigs.TeamsCount * gameConfigs.TeamSize)
            {
                throw new ApiException(InvalidRequest);
            }
        }
    }
}
