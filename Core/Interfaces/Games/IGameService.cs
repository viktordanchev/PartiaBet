using Core.Models.Games;
using Core.Models.Games.Chess;
using Core.Models.Match;

namespace Core.Interfaces.Games
{
    public interface IGameService
    {
        GameBoardModel CreateGameBoard(IEnumerable<PlayerModel> players);
        void UpdateBoard(GameBoardModel board, GameMoveModel move);
        bool IsValidMove(GameBoardModel board, GameMoveModel move);
        bool IsWinningMove(GameBoardModel board, GameMoveModel move);
        IEnumerable<PlayerModel> UpdateWinners(IEnumerable<PlayerModel> players, Guid winnerMoveId);
        Guid SwitchTurn(Guid playerId, IEnumerable<PlayerModel> players);
    }
}