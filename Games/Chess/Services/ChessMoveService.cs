using Core.Games.Enums;
using Core.Models.Games.Chess;
using Games.Dtos.Chess;

namespace Games.Chess.Services
{
    public class ChessMoveService
    {
        public static bool IsValidMove(ChessBoardModel board, ChessMoveDto move)
        {
            if (move.NewRow < 0 || move.NewRow > 7 || move.NewCol < 0 || move.NewCol > 7)
            {
                return false;
            }

            var piece = board.Pieces.FirstOrDefault(p => p.Row == move.NewRow && p.Col == move.NewCol);

            bool isValidMove;
            switch (move.PieceType)
            {
                case PieceType.King:
                    isValidMove = IsValidKingMove(board, move);
                    break;
                case PieceType.Queen:
                    isValidMove = IsValidQueenMove(board, move);
                    break;
                case PieceType.Rook:
                    isValidMove = IsValidRookMove(board, move);
                    break;
                case PieceType.Knight:
                    isValidMove = IsValidKnightMove(board, move);
                    break;
                case PieceType.Bishop:
                    isValidMove = IsValidBishopMove(board, move);
                    break;
                case PieceType.Pawn:
                    var isWhite = move.NewRow > move.OldRow;
                    isValidMove = IsValidPawnMove(board, move, isWhite);
                    break;
                default:
                    isValidMove = false;
                    break;
            }

            return isValidMove;
        }

        //private methods

        private static bool IsValidKingMove(ChessBoardModel board, ChessMoveDto move)
        {
            var directions = new List<(int Row, int Col)>
            {
                (1, 0),
                (1, 1),
                (1, -1),
                (-1, 0),
                (-1, 1),
                (-1, -1),
                (0, 1),
                (0, -1)
            };

            return GetSingleMoves(move, directions);
        }

        private static bool IsValidQueenMove(ChessBoardModel board, ChessMoveDto move)
        {
            var directions = new List<(int Row, int Col)>
            {
                (1, 1),
                (1, -1),
                (-1, -1),
                (-1, 1),
                (1, 0),
                (-1, 0),
                (0, 1),
                (0, -1)
            };

            return GetLinearMoves(board, move, directions);
        }

        private static bool IsValidRookMove(ChessBoardModel board, ChessMoveDto move)
        {
            var directions = new List<(int Row, int Col)>
            {
                (1, 0),
                (-1, 0),
                (0, 1),
                (0, -1)
            };

            return GetLinearMoves(board, move, directions);
        }

        private static bool IsValidKnightMove(ChessBoardModel board, ChessMoveDto move)
        {
            var directions = new List<(int Row, int Col)>
            {
                (2, 1),
                (2, -1),
                (-2, 1),
                (-2, -1),
                (1, 2),
                (-1, 2),
                (1, -2),
                (-1, -2)
            };

            return GetSingleMoves(move, directions);
        }

        private static bool IsValidBishopMove(ChessBoardModel board, ChessMoveDto move)
        {
            var directions = new List<(int Row, int Col)>
            {
                (1, 1),
                (1, -1),
                (-1, -1),
                (-1, 1)
            };

            return GetLinearMoves(board, move, directions);
        }

        private static bool IsValidPawnMove(ChessBoardModel board, ChessMoveDto move, bool isWhite)
        {
            var directions = new List<(int row, int col)>
            {
                (isWhite ? 1 : -1, 0),
                (isWhite ? 1 : -1, 1),
                (isWhite ? 1 : -1, -1)
            };

            if ((isWhite && move.NewRow == 1) || (!isWhite && move.NewRow == 6))
            {
                directions.Add((isWhite ? 2 : -2, 0));
            }

            return GetSingleMoves(move, directions);
        }

        private static bool GetLinearMoves(ChessBoardModel board, ChessMoveDto move, List<(int Row, int Col)> directions)
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

        private static bool GetSingleMoves(ChessMoveDto move, List<(int Row, int Col)> directions)
        {
            var isValidMove = false;

            foreach (var dir in directions)
            {
                var row = move.OldRow + dir.Row;
                var col = move.OldCol + dir.Col;

                if (row < 0 || row >= 8 || col < 0 || col >= 8) break;

                if (row == move.NewRow && col == move.NewCol)
                {
                    isValidMove = true;
                    break;
                }
            }

            return isValidMove;
        }
    }
}
