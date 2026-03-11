using Core.Models.Games;
using Core.Models.Match;

namespace Core.Interfaces.Games
{
    public interface IGameService
    {
        GameBoardModel CreateGameBoard(IEnumerable<PlayerModel> players);
        void UpdateBoard(GameBoardModel board, GameMoveModel move);
        bool IsValidMove(GameBoardModel board, GameMoveModel move);
        bool IsWinningMove(GameBoardModel board, GameMoveModel move);
        void UpdateWinners(IEnumerable<PlayerModel> players, Guid winnerId);
        Guid SwitchTurn(Guid playerId, IEnumerable<PlayerModel> players);
    }
}