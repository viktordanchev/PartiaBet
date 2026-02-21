using Core.Models.Games.Chess;
using Games.Chess.Interfaces;

namespace Games.Chess.PieceMovement.Validators
{
    public class PawnMoveValidator : IPieceMoveValidator
    {
        public bool IsValidMove(ChessBoardModel board, ChessMoveModel move)
        {
            var pawn = board.Pieces.FirstOrDefault(p => p.Row == move.OldRow && p.Col == move.OldCol);
            if (pawn == null)
                return false;

            int direction = pawn.IsWhite ? 1 : -1;

            // 1️⃣ Нормално движение напред
            if (IsForwardMove(board, move, direction))
                return true;

            // 2️⃣ Двоен ход от начална позиция
            if (IsDoubleForwardMove(board, move, pawn, direction))
                return true;

            // 3️⃣ Взимане на противникова фигура по диагонал
            if (IsCaptureMove(board, move, pawn, direction))
                return true;

            return false;
        }

        private bool IsForwardMove(ChessBoardModel board, ChessMoveModel move, int direction)
        {
            // Пешката се движи напред само ако квадратът е празен
            if (move.NewCol == move.OldCol && move.NewRow == move.OldRow + direction)
            {
                return !IsOccupied(board, move.NewRow, move.NewCol);
            }
            return false;
        }

        private bool IsDoubleForwardMove(ChessBoardModel board, ChessMoveModel move, FigureModel pawn, int direction)
        {
            // Може само ако е на началната си позиция
            int startingRow = pawn.IsWhite ? 1 : 6;
            if (move.OldRow != startingRow || move.NewCol != move.OldCol)
                return false;

            if (move.NewRow == move.OldRow + 2 * direction)
            {
                // Проверка дали и двата квадрата са свободни
                int intermediateRow = move.OldRow + direction;
                return !IsOccupied(board, intermediateRow, move.OldCol) &&
                       !IsOccupied(board, move.NewRow, move.NewCol);
            }

            return false;
        }

        private bool IsCaptureMove(ChessBoardModel board, ChessMoveModel move, FigureModel pawn, int direction)
        {
            // Пешката взима диагонално, ако има противникова фигура
            if (Math.Abs(move.NewCol - move.OldCol) == 1 && move.NewRow == move.OldRow + direction)
            {
                var target = board.Pieces.FirstOrDefault(p => p.Row == move.NewRow && p.Col == move.NewCol);
                return target != null && target.IsWhite != pawn.IsWhite;
            }

            return false;
        }

        private bool IsOccupied(ChessBoardModel board, int row, int col)
        {
            return board.Pieces.Any(p => p.Row == row && p.Col == col);
        }
    }
}
