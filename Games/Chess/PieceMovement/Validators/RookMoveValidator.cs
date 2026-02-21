using Core.Models.Games.Chess;
using Games.Chess.Interfaces;

namespace Games.Chess.PieceMovement.Validators
{
    public class RookMoveValidator : IPieceMoveValidator
    {
        public bool IsValidMove(ChessBoardModel board, ChessMoveModel move)
        {
            return MovementHelper.IsLinearMoveValid(board, move, Directions.Rook);
        }
    }
}
