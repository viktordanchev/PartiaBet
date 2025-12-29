using Core.Models.Games;

namespace Core.Interfaces.Infrastructure
{
    public interface IGamesRepository
    {
        Task<IEnumerable<GameModel>> GetAllAsync();
        Task<GameModel?> GetGameAsync(string game);
    }
}
