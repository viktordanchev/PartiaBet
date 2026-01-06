using Core.Models.Games;

namespace Core.Interfaces.Games
{
    public interface IGameService
    {
        GameBoardModel CreateGameBoard();
        void UpdateBoard(GameBoardModel board, BaseMoveModel move);
        bool IsValidMove(GameBoardModel board, BaseMoveModel move);
        void UpdatePlayersInBoard(GameBoardModel board, Guid playerId);
        bool IsWinningMove(GameBoardModel board);
    }
}