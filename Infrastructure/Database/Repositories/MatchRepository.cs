using Core.DTOs.Requests.Matches;
using Core.Interfaces.Repositories;
using Infrastructure.Database.Entities;
using System.Globalization;

namespace Infrastructure.Database.Repositories
{
    public class MatchRepository : IMatchRepository
    {
        private readonly PartiaBetDbContext _context;

        public MatchRepository(PartiaBetDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> AddMatch(AddMatchRequest match)
        {
            var newMatch = new Match()
            {
                BetAmount = match.BetAmount,
                DateAndTime = DateTime.Parse(match.DateAndTime, new CultureInfo("bg-BG")),
                GameId = match.GameId,
            };

            await _context.MatchHistory.AddAsync(newMatch);
            await _context.SaveChangesAsync();

            return newMatch.Id;
        }
    }
}
