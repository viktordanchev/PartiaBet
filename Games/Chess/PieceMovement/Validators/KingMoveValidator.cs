using Core.Models.Games.Chess;
using Games.Chess.Interfaces;
using Games.Chess.Services;

namespace Games.Chess.PieceMovement.Validators
{
    public class KingMoveValidator : IPieceMoveValidator
    {
        public bool IsValidMove(ChessBoardModel board, ChessMoveModel move)
        {
            var king = board.Pieces.FirstOrDefault(p => p.Row == move.OldRow && p.Col == move.OldCol);
            if (king == null)
                return false;

            if (!MovementHelper.IsSingleMoveValid(move, Directions.King))
                return false;

            if (ChessAttackDetector.IsSquareAttacked(board, move.NewRow, move.NewCol, king.IsWhite))
                return false;

            var target = board.Pieces.FirstOrDefault(p => p.Row == move.NewRow && p.Col == move.NewCol);
            if (target != null && target.IsWhite == king.IsWhite)
                return false;

            return true;
        }
    }
}
