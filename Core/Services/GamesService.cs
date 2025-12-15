using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models.Games;

namespace Core.Services
{
    public class GamesService : IGamesService
    {
        private readonly IGamesRepository _gamesRepository;

        public GamesService(IGamesRepository gamesRepository)
        {
            _gamesRepository = gamesRepository;
        }

        public async Task<IEnumerable<GameModel>> GetAllAsync()
        {
            return await _gamesRepository.GetAllAsync();
        }

        public async Task<GameModel?> GetGameAsync(string game)
        {
            return await _gamesRepository.GetGameAsync(game);
        }
    }
}
