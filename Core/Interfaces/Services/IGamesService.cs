using Core.Models.Games;

namespace Core.Interfaces.Services
{
    public interface IGamesService
    {
        Task<IEnumerable<GameModel>> GetAllAsync();
        Task<GameModel?> GetGameAsync(string game);
    }
}
