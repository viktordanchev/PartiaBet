using Core.Models.Games.Chess;
using Games.Chess.Interfaces;

namespace Games.Chess.PieceMovement.Validators
{
    public class KnightMoveValidator : IPieceMoveValidator
    {
        public bool IsValidMove(ChessBoardModel board, ChessMoveModel move)
        {
            return MovementHelper.IsSingleMoveValid(move, Directions.Knight);
        }
    }
}
