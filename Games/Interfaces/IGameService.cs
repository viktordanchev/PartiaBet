using Games.Dtos;
using Games.Models;

namespace Games.Interfaces
{
    public interface IGameService
    {
        IGameConfigs Configs { get; }
        GameBoardModel CreateGameBoard();
        void AddToBoard(Guid playerId, GameBoardModel board);
        void UpdateBoard(GameBoardModel board, BaseMakeMoveDto move);
    }
}
