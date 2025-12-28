using Core.Models.Match;

namespace Core.Interfaces.Games
{
    public interface IGameService
    {
        GameBoardModel CreateGameBoard();
        void UpdateBoard(GameBoardModel board, BaseMoveModel move);
        bool IsValidMove(GameBoardModel board, BaseMoveModel move);
        void AddPlayerToBoard(GameBoardModel board, Guid playerId, int playersCount);
    }
}