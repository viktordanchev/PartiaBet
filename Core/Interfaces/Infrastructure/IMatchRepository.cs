using Core.Models.Match;

namespace Core.Interfaces.Infrastructure
{
    public interface IMatchRepository
    {
        Task<PlayerModel> GetPlayerDataAsync(Guid playerId);
    }
}
