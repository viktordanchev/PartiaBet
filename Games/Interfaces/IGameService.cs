using Games.Dtos.MatchManagerService;
using Games.Models;

namespace Games.Interfaces
{
    public interface IGameService
    {
        IGameConfigs Configs { get; }
        GameBoardModel CreateGameBoard();
        void AddToBoard(Guid playerId, GameBoardModel board);
        void UpdateBoard(GameBoardModel board, BaseMoveDto move);
        bool IsValidMove(GameBoardModel board, BaseMoveDto move);
    }
}
