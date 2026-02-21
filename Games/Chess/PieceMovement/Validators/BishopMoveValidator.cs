using Core.Models.Games.Chess;
using Games.Chess.Interfaces;

namespace Games.Chess.PieceMovement.Validators
{
    public class BishopMoveValidator : IPieceMoveValidator
    {
        public bool IsValidMove(ChessBoardModel board, ChessMoveModel move)
        {
            return MovementHelper.IsLinearMoveValid(board, move, Directions.Bishop);
        }
    }
}
