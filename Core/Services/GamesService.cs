using Core.DTOs.Responses.Games;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;

namespace Core.Services
{
    public class GamesService : IGamesService
    {
        private readonly IGamesRepository _gamesRepository;

        public GamesService(IGamesRepository gamesRepository)
        {
            _gamesRepository = gamesRepository;
        }

        public async Task<IEnumerable<GameResponse>> GetAllAsync()
        {
            return await _gamesRepository.GetAllAsync();
        }
    }
}
