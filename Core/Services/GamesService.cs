using Core.Interfaces.Games;
using Core.Interfaces.Services;
using Core.Models.Games;

namespace Core.Services
{
    public class GamesService : IGamesService
    {
        private readonly IGameProvider _gameProvider;

        public GamesService(IGameProvider gameProvider)
        {
            _gameProvider = gameProvider;
        }

        public IEnumerable<GameModel> GetAll()
        {
            return _gameProvider.GenerateAllGames();
        }

        public GameModel? GetGame(string game)
        {
            var allGames = _gameProvider.GenerateAllGames();

            return allGames.FirstOrDefault(g => g.Name.Equals(game, StringComparison.OrdinalIgnoreCase));
        }
    }
}
