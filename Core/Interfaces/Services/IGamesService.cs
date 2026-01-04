using Core.Models.Games;

namespace Core.Interfaces.Services
{
    public interface IGamesService
    {
        IEnumerable<GameModel> GetAll();
        GameModel? GetGame(string game);
    }
}
