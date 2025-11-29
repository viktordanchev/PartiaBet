using Games.Models;

namespace Games.Interfaces
{
    public interface IGameService
    {
        IGameConfigs Configs { get; }
        GameBoardModel CreateGameBoard();
        void AddToBoard(Guid playerId, GameBoardModel board);
    }
}
