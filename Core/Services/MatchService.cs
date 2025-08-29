using Core.Games.Dtos;
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

        public async Task AddMatchAsync(MatchDto match)
        {
            match.Id = await _matchRepository.AddMatch(match);
        }
    }
}
