using Core.Models.Games;
using Core.Models.Match;

namespace Core.Interfaces.Games
{
    public interface IGameService
    {
        GameBoardModel CreateGameBoard();
        void UpdateBoard(GameBoardModel board, BaseMoveModel move);
        bool IsValidMove(GameBoardModel board, BaseMoveModel move);
        void UpdatePlayersInBoard(GameBoardModel board, Guid playerId);
        bool IsWinningMove(GameBoardModel board);
        Guid SwitchTurn(Guid playerId, IEnumerable<PlayerModel> players);
    }
}