using Core.Models.Games;

namespace Core.Interfaces.Infrastructure
{
    public interface ICacheService
    {
        Task AddItem(Guid matchId, GameBoardModel gameBoard);
        Task<GameBoardModel> GetItem(Guid matchId);
    }
}
