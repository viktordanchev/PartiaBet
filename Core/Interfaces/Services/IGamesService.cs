using Core.DTOs.Responses.Games;

namespace Core.Interfaces.Services
{
    public interface IGamesService
    {
        Task<IEnumerable<GameResponse>> GetAllAsync();
        Task<GameDetailsResponse?> GetDetailsAsync(string game);
    }
}
