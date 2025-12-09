using Core.Dtos.Games;

namespace Core.Interfaces.Repositories
{
    public interface IGamesRepository
    {
        Task<IEnumerable<GameDto>> GetAllAsync();
        Task<GameDto?> GetGameAsync(string game);
    }
}
