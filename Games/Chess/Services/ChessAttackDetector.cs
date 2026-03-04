using Core.Games.Enums;
using Core.Models.Games.Chess;
using Games.Chess.PieceMovement;

namespace Games.Chess.Services
{
    public static class ChessAttackDetector
    {
        public static bool IsSquareAttacked(
            ChessBoardModel board,
            int targetRow,
            int targetCol,
            bool isWhite)
        {
            var enemyPieces = board.Pieces
                .Where(p => p.IsWhite != isWhite);

            foreach (var piece in enemyPieces)
            {
                switch (piece.Type)
                {
                    case PieceType.Pawn:
                        if (IsPawnAttackingSquare(piece, targetRow, targetCol))
                            return true;
                        break;

                    case PieceType.Knight:
                        if (IsSingleStepAttack(piece, targetRow, targetCol, Directions.Knight))
                            return true;
                        break;

                    case PieceType.Bishop:
                        if (IsLinearAttack(board, piece, targetRow, targetCol, Directions.Bishop))
                            return true;
                        break;

                    case PieceType.Rook:
                        if (IsLinearAttack(board, piece, targetRow, targetCol, Directions.Rook))
                            return true;
                        break;

                    case PieceType.Queen:
                        if (IsLinearAttack(board, piece, targetRow, targetCol, Directions.Queen))
                            return true;
                        break;

                    case PieceType.King:
                        if (IsSingleStepAttack(piece, targetRow, targetCol, Directions.King))
                            return true;
                        break;
                }
            }

            return false;
        }

        //private methods

        private static bool IsPawnAttackingSquare(
            FigureModel pawn,
            int targetRow,
            int targetCol)
        {
            int direction = pawn.IsWhite ? 1 : -1;

            return
                pawn.Row + direction == targetRow &&
                (pawn.Col + 1 == targetCol || pawn.Col - 1 == targetCol);
        }

        private static bool IsSingleStepAttack(
            FigureModel piece,
            int targetRow,
            int targetCol,
            List<(int Row, int Col)> directions)
        {
            foreach (var dir in directions)
            {
                int row = piece.Row + dir.Row;
                int col = piece.Col + dir.Col;

                if (row == targetRow && col == targetCol)
                    return true;
            }

            return false;
        }

        private static bool IsLinearAttack(
            ChessBoardModel board,
            FigureModel piece,
            int targetRow,
            int targetCol,
            List<(int Row, int Col)> directions)
        {
            foreach (var dir in directions)
            {
                for (int i = 1; i < 8; i++)
                {
                    int row = piece.Row + dir.Row * i;
                    int col = piece.Col + dir.Col * i;

                    if (row < 0 || row > 7 || col < 0 || col > 7)
                        break;

                    if (row == targetRow && col == targetCol)
                        return true;

                    if (board.Pieces.Any(p => p.Row == row && p.Col == col))
                        break;
                }
            }

            return false;
        }
    }
}
