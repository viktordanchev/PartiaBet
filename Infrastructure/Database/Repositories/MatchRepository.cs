using Core.Games.Dtos;
using Core.Interfaces.Repositories;
using Infrastructure.Database.Entities;

namespace Infrastructure.Database.Repositories
{
    public class MatchRepository : IMatchRepository
    {
        private readonly PartiaBetDbContext _context;

        public MatchRepository(PartiaBetDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> AddMatch(MatchDto match)
        {
            var newMatch = new Match()
            {
                BetAmount = match.BetAmount,
                DateAndTime = match.DateAndTime,
                GameId = match.GameId,
            };

            await _context.MatchHistory.AddAsync(newMatch);
            await _context.SaveChangesAsync();

            return newMatch.Id;
        }
    }
}
