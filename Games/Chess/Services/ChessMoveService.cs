using Core.Games.Enums;
using Core.Models.Games.Chess;

namespace Games.Chess.Services
{
    public class ChessMoveService
    {
        public static bool IsValidMove(ChessBoardModel board, ChessMoveModel move)
        {
            var currPiece = board.Pieces.FirstOrDefault(p => p.Row == move.OldRow && p.Col == move.OldCol);
            var targetPiece = board.Pieces.FirstOrDefault(p => p.Row == move.NewRow && p.Col == move.NewCol);

            if (targetPiece.Row < 0 || targetPiece.Row > 7 || targetPiece.Col < 0 || targetPiece.Col > 7)
            {
                return false;
            }

            if (currPiece == null)
            {
                return false;
            }

            if (IsMoveBlockedByOwnPiece(board, currPiece, targetPiece))
            {
                return false;
            }

            bool isValidMove;
            switch (currPiece.Type)
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

        private static bool IsMoveBlockedByOwnPiece(ChessBoardModel board, FigureModel currPiece, FigureModel targetPiece)
        {
            if (currPiece != null && targetPiece != null)
            {
                if (currPiece.IsWhite == targetPiece.IsWhite)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsValidKingMove(ChessBoardModel board, ChessMoveModel move)
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

        private static bool IsValidQueenMove(ChessBoardModel board, ChessMoveModel move)
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

        private static bool IsValidRookMove(ChessBoardModel board, ChessMoveModel move)
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

        private static bool IsValidKnightMove(ChessBoardModel board, ChessMoveModel move)
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

        private static bool IsValidBishopMove(ChessBoardModel board, ChessMoveModel move)
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

        private static bool IsValidPawnMove(ChessBoardModel board, ChessMoveModel move, bool isWhite)
        {
            var directions = new List<(int row, int col)>
            {
                (isWhite ? 1 : -1, 0),
                (isWhite ? 1 : -1, 1),
                (isWhite ? 1 : -1, -1)
            };

            if ((isWhite && move.OldRow == 1) || (!isWhite && move.OldRow == 6))
            {
                directions.Add((isWhite ? 2 : -2, 0));
            }

            return GetSingleMoves(move, directions);
        }

        private static bool GetLinearMoves(ChessBoardModel board, ChessMoveModel move, List<(int Row, int Col)> directions)
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

        private static bool GetSingleMoves(ChessMoveModel move, List<(int Row, int Col)> directions)
        {
            var isValidMove = false;

            foreach (var dir in directions)
            {
                var row = move.OldRow + dir.Row;
                var col = move.OldCol + dir.Col;

                if (row < 0 || row >= 8 || col < 0 || col >= 8) continue;

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
