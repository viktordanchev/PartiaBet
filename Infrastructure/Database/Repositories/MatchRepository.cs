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

        public async Task<PlayerModel> GetPlayerDataAsync(Guid playerId)
        {
            var playerEntity = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == playerId);

            return new PlayerModel
            {
                Id = playerEntity.Id,
                ProfileImageUrl = playerEntity.ImageUrl,
                Username = playerEntity.Username,
                Rating = 1000
            };
        }
    }
}
