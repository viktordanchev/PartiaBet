using Core.Models.Games.Chess;

namespace Games.Chess.PieceMovement
{
    public static class MovementHelper
    {
        public static bool IsLinearMoveValid(ChessBoardModel board, ChessMoveModel move, List<(int Row, int Col)> directions)
        {
            foreach (var dir in directions)
            {
                for (var i = 1; i < 8; i++)
                {
                    var row = move.OldRow + dir.Row * i;
                    var col = move.OldCol + dir.Col * i;

                    if (row < 0 || row >= 8 || col < 0 || col >= 8) break;

                    if (row == move.NewRow && col == move.NewCol) return true;

                    if (board.Pieces.Any(p => p.Row == row && p.Col == col)) break;
                }
            }

            return false;
        }

        public static bool IsSingleMoveValid(ChessMoveModel move, List<(int Row, int Col)> directions)
        {
            foreach (var dir in directions)
            {
                var row = move.OldRow + dir.Row;
                var col = move.OldCol + dir.Col;

                if (row < 0 || row >= 8 || col < 0 || col >= 8) continue;

                if (row == move.NewRow && col == move.NewCol)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
