using Core.Interfaces.Infrastructure;
using Core.Models.Games;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Repositories
{
    public class GamesRepository : IGamesRepository
    {
        private readonly PartiaBetDbContext _context;

        public GamesRepository(PartiaBetDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GameModel>> GetAllAsync()
        {
            return await _context.Games
                .AsNoTracking()
                .Select(g => new GameModel()
                {
                    Id = g.Id,
                    Name = g.Name,
                    ImageUrl = g.ImgUrl
                })
                .ToListAsync();
        }

        public async Task<GameModel?> GetGameAsync(string game)
        {
            return await _context.Games
                .AsNoTracking()
                .Where(g => g.Name.ToLower() == game.ToLower())
                .Select(g => new GameModel()
                {
                    Id = g.Id,
                    Name = g.Name,
                    ImageUrl = g.ImgUrl
                })
                .FirstOrDefaultAsync();
        }
    }
}
