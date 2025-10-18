using Core.DTOs.Requests.Matches;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;

namespace Core.Services
{
    public class MatchService : IMatchService
    {
        private readonly IMatchRepository _matchRepository;

        public MatchService(IMatchRepository matchRepository)
        {
            _matchRepository = matchRepository;
        }

        public async Task<Guid> AddMatchAsync(AddMatchRequest match)
        {
            return await _matchRepository.AddMatch(match);
        }
    }
}
