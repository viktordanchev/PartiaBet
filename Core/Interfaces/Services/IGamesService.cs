using Core.Dtos.Games;

namespace Core.Interfaces.Services
{
    public interface IGamesService
    {
        Task<IEnumerable<GameDto>> GetAllAsync();
        Task<GameDto?> GetGameAsync(string game);
    }
}
