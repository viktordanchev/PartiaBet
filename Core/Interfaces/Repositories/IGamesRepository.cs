using Core.Models.Games;

namespace Core.Interfaces.Repositories
{
    public interface IGamesRepository
    {
        Task<IEnumerable<GameModel>> GetAllAsync();
        Task<GameModel?> GetGameAsync(string game);
    }
}
