using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models.Match;

namespace Core.Services
{
    public class MatchService : IMatchService
    {
        private IMatchRepository _matchRepository;
        //private DBREDS

        public MatchService(IMatchRepository matchRepository)
        {
            _matchRepository = matchRepository;
        }

        public async Task<IEnumerable<MatchModel>> GetActiveMatchesAsync(int gameId)
        {
            return await _matchRepository.GetActiveMatchesAsync(gameId);
        }

        public async Task<MatchModel> AddMatchAsync(AddMatchModel data)
        {
            var matchId = await _matchRepository.AddMatchAsync(data);

            return new MatchModel()
            {
               
            };
        }

        public async Task AddPersonToMatch(Guid matchId, Guid playerId)
        {
            await _matchRepository.TryAddPlayerToMatchAsync(playerId, matchId);
        }

        public async Task<MatchDetailsModel> GetMatch(Guid matchId)
        {
            var match = await _matchRepository.GetMatchDetailsAsync(matchId);

            return match;
        }
    }
}
