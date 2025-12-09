using Core.Dtos.Games;
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

        public async Task<IEnumerable<GameDto>> GetAllAsync()
        {
            return await _gamesRepository.GetAllAsync();
        }

        public async Task<GameDto?> GetGameAsync(string game)
        {
            return await _gamesRepository.GetGameAsync(game);
        }
    }
}
