using Core.Interfaces.Repositories;
using Core.Models.Match;
using Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Repositories
{
    public class MatchRepository : IMatchRepository
    {
        private readonly PartiaBetDbContext _context;

        public MatchRepository(PartiaBetDbContext context)
        {
            _context = context;
        }

        public async Task<MatchModel> AddMatchAsync(AddMatchModel data)
        {
            var newMatch = new Match()
            {
                BetAmount = data.BetAmount,
                //DateAndTime = DateTime.Parse(data.DateAndTime, new CultureInfo("bg-BG")),
                GameId = data.GameId,
                IsActive = true
            };

            await _context.MatchHistory.AddAsync(newMatch);
            await _context.SaveChangesAsync();
            await _context.Entry(newMatch)
                .Reference(m => m.Game)
                .LoadAsync();

            return new MatchModel()
            {
                Id = newMatch.Id,
                BetAmount = newMatch.BetAmount,
                MaxPlayersCount = newMatch.Game.MaxPlayersCount,
                Players = new List<PlayerModel>()
            };
        }

        public async Task<PlayerModel> TryAddPlayerToMatchAsync(Guid playerId, Guid matchId)
        {
            var match = await _context.MatchHistory.FindAsync(matchId);

            if (match != null)
            {
                match.Players.Add(
                    new UserMatch()
                    {
                        PlayerId = playerId,
                        MatchId = matchId
                    });

                await _context.SaveChangesAsync();
            }

            var addedPlayer = await _context.Users.FindAsync(playerId);

            return new PlayerModel()
            {
                Id = addedPlayer.Id,
                ProfileImageUrl = addedPlayer.ImageUrl,
                Rating = 1000,
                Username = addedPlayer.Username
            };
        }

        public async Task TryRemovePlayerFromMatchAsync(Guid playerId, Guid matchId)
        {
            var userMatch = await _context.UserMatch
                .FirstOrDefaultAsync(um => um.PlayerId == playerId && um.MatchId == matchId);

            if (userMatch != null)
            {
                _context.UserMatch.Remove(userMatch);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<MatchModel>> GetActiveMatchesAsync(int gameId)
        {
            var matches = await _context.MatchHistory
                .Where(m => m.GameId == gameId && m.IsActive)
                .Select(m => new MatchModel()
                {
                    Id = m.Id,
                    BetAmount = m.BetAmount,
                    MaxPlayersCount = m.Game.MaxPlayersCount,
                    Players = m.Players
                        .Select(um => new PlayerModel()
                        {
                            Id = um.PlayerId,
                            Username = um.Player.Username,
                            ProfileImageUrl = um.Player.ImageUrl,
                            Rating = 1000
                        })
                        .ToList()
                })
                .ToListAsync();

            return matches;
        }

        public async Task<MatchDetailsModel> GetMatchDetailsAsync(Guid matchId)
        {
            var match = await _context.MatchHistory
                .Where(m => m.Id == matchId)
                .Select(m => new MatchDetailsModel()
                {
                    BetAmount = m.BetAmount,
                    MaxPlayersCount = m.Game.MaxPlayersCount,
                    Players = m.Players
                        .Select(um => new PlayerModel()
                        {
                            Id = um.PlayerId,
                            Username = um.Player.Username,
                            ProfileImageUrl = um.Player.ImageUrl,
                            Rating = 1000
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();

            return match;
        }

        public async Task<int> GetGameIdAsync(Guid matchId)
        {
            return await _context.MatchHistory
                .Where(m => m.Id == matchId)
                .Select(m => m.GameId)
                .FirstOrDefaultAsync();
        }

        public async Task<int> GetPlayersCountAsync(Guid matchId)
        {
            return await _context.MatchHistory
                .Where(m => m.Id == matchId)
                .Select(m => m.Players.Count)
                .FirstAsync();
        }
    }
}
