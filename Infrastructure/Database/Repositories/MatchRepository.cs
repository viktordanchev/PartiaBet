using Core.Enums;
using Core.Interfaces.Infrastructure;
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
                DateAndTime = DateTime.UtcNow,
                GameType = data.GameType,
                MatchStatus = MatchStatus.Created
            };

            await _context.MatchHistory.AddAsync(newMatch);
            await _context.SaveChangesAsync();

            return new MatchModel()
            {
                Id = newMatch.Id,
                BetAmount = newMatch.BetAmount,
                GameType = newMatch.GameType,
                MatchStatus = newMatch.MatchStatus,
                Players = new List<PlayerModel>()
            };
        }

        public async Task<PlayerModel> AddPlayerAsync(Guid playerId, Guid matchId)
        {
            var match = await _context.MatchHistory
                .Include(m => m.Players)
                .FirstOrDefaultAsync(m => m.Id == matchId);

            if (match != null)
            {
                match.Players.Add(
                    new UserMatch()
                    {
                        PlayerId = playerId,
                        MatchId = matchId,
                        TurnOrder = match.Players.Count + 1,
                        Status = PlayerStatus.Active,
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

        public async Task RemovePlayerAsync(Guid playerId, Guid matchId)
        {
            var userMatch = await _context.UserMatch
                .FirstOrDefaultAsync(um => um.PlayerId == playerId && um.MatchId == matchId);

            if (userMatch != null)
            {
                _context.UserMatch.Remove(userMatch);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<MatchModel>> GetActiveMatchesAsync(GameType gameType)
        {
            var matches = await _context.MatchHistory
                .Where(m => m.GameType == gameType && (m.MatchStatus == MatchStatus.Created || m.MatchStatus == MatchStatus.Ongoing))
                .Select(m => new MatchModel()
                {
                    Id = m.Id,
                    BetAmount = m.BetAmount,
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

        public async Task<MatchModel> GetMatchAsync(Guid matchId)
        {
            var match = await _context.MatchHistory
                .Where(m => m.Id == matchId)
                .Select(m => new MatchModel()
                {
                    Id = m.Id,
                    BetAmount = m.BetAmount,
                    CurrentTurnPlayerId = m.CurrentTurnPlayerId,
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

        public async Task<MatchModel> GetMatchInternalAsync(Guid matchId)
        {
            var match = await _context.MatchHistory
                .Where(m => m.Id == matchId)
                .Select(m => new MatchModel()
                {
                    Id = m.Id,
                    GameType = m.GameType,
                    MatchStatus = m.MatchStatus,
                    Players = m.Players
                        .Select(um => new PlayerModel()
                        {
                            Id = um.PlayerId,
                            TurnOrder = um.TurnOrder,
                            TeamNumber = um.TeamNumber
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();

            return match;
        }

        public async Task UpdateMatchStatusAsync(Guid matchId, MatchStatus newStatus)
        {
            var match = await _context.MatchHistory.FindAsync(matchId);

            if (match != null)
            {
                match.MatchStatus = newStatus;
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdatePlayerStatusAsync(Guid matchId, Guid playerId, PlayerStatus newStatus)
        {
            var userMatch = await _context.UserMatch
                .FirstOrDefaultAsync(um => um.MatchId == matchId && um.PlayerId == playerId);

            if (userMatch != null)
            {
                userMatch.Status = newStatus;
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdatePlayerIdAsync(Guid matchId, Guid newPlayerId)
        {
            var userMatch = await _context.MatchHistory
                .FirstOrDefaultAsync(um => um.Id == matchId);

            if (userMatch != null)
            {
                userMatch.CurrentTurnPlayerId = newPlayerId;
                await _context.SaveChangesAsync();
            }
        }
    }
}
