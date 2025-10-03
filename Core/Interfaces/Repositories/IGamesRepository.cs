using Core.DTOs.Responses.Games;

namespace Core.Interfaces.Repositories
{
    public interface IGamesRepository
    {
        Task<IEnumerable<GameResponse>> GetAllAsync();
        Task<GameDetailsResponse?> GetDetailsAsync(string game);
    }
}
