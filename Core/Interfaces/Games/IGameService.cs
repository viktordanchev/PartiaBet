using Core.Models.Match;

namespace Core.Interfaces.Games
{
    public interface IGameService
    {
        GameBoardModel CreateGameBoard();
        void AddToBoard(Guid playerId, GameBoardModel board);
        void UpdateBoard(GameBoardModel board, BaseMoveModel move);
        bool IsValidMove(GameBoardModel board, BaseMoveModel move, string playerId);
    }
}