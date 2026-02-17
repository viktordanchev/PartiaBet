using Core.Games.Enums;
using Core.Models.Games.Chess;

namespace Games.Chess.Services
{
    public class ChessMoveService
    {
        public static bool IsValidMove(ChessBoardModel board, ChessMoveModel move)
        {
            if (move.NewRow < 0 || move.NewRow > 7 ||
                move.NewCol < 0 || move.NewCol > 7)
            {
                return false;
            }

            var currPiece = board.Pieces.FirstOrDefault(p => p.Row == move.OldRow && p.Col == move.OldCol);
            var targetPiece = board.Pieces.FirstOrDefault(p => p.Row == move.NewRow && p.Col == move.NewCol);

            if (currPiece == null)
            {
                return false;
            }

            if (currPiece != null && targetPiece != null)
            {
                if (IsCastleMove(board, currPiece, targetPiece))
                {
                    return CanPerformCastle(board, move);
                }
                else if (IsMoveBlockedByOwnPiece(board, currPiece, targetPiece))
                {
                    return false;
                }
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
                    var isWhite = currPiece.IsWhite;
                    isValidMove = IsValidPawnMove(board, move, isWhite);
                    break;
                default:
                    isValidMove = false;
                    break;
            }

            return isValidMove;
        }

        public static bool IsWinningMove(ChessBoardModel board)
        {
            var kingsCount = board.Pieces.Count(p => p.Type == PieceType.King);

            return kingsCount < 2;
        }

        public static void UpdateCastlingRights(ChessBoardModel board, FigureModel currPiece)
        {
            if (currPiece.Type == PieceType.King)
            {
                if (currPiece.IsWhite)
                {
                    board.CanWhiteSmallCastle = false;
                    board.CanWhiteBigCastle = false;
                }
                else
                {
                    board.CanBlackSmallCastle = false;
                    board.CanBlackBigCastle = false;
                }
            }

            if (currPiece.Type == PieceType.Rook)
            {
                if (currPiece.IsWhite && currPiece.Row == 0)
                {
                    if (currPiece.Col == 7)
                        board.CanWhiteBigCastle = false;

                    if (currPiece.Col == 0)
                        board.CanWhiteSmallCastle = false;
                }

                if (!currPiece.IsWhite && currPiece.Row == 7)
                {
                    if (currPiece.Col == 7)
                        board.CanBlackBigCastle = false;

                    if (currPiece.Col == 0)
                        board.CanBlackSmallCastle = false;
                }
            }
        }

        public static void PerformCastle(ChessBoardModel board, FigureModel king, FigureModel rook)
        {
            var isWhite = king.IsWhite;
            var isBigCastle = rook.Col == 7;
            var canSmallCastle = isWhite ? board.CanWhiteSmallCastle : board.CanBlackSmallCastle;
            var canBigCastle = isWhite ? board.CanWhiteBigCastle : board.CanBlackBigCastle;

            if (canSmallCastle && !isBigCastle)
            {
                king.Col = 1;
                rook.Col = 2;
            }
            else if (canBigCastle && isBigCastle)
            {
                king.Col = 6;
                rook.Col = 5;
            }

            if (isWhite)
            {
                board.CanWhiteSmallCastle = false;
                board.CanWhiteBigCastle = false;
            }
            else
            {
                board.CanBlackSmallCastle = false;
                board.CanBlackBigCastle = false;
            }
        }

        public static bool IsCastleMove(ChessBoardModel board, FigureModel currPiece, FigureModel targetPiece)
        {
            return currPiece != null && targetPiece != null &&
                currPiece.Type == PieceType.King && targetPiece.Type == PieceType.Rook &&
                currPiece.IsWhite == targetPiece.IsWhite;
        }

        //private methods

        private static bool CanPerformCastle(ChessBoardModel board, ChessMoveModel move)
        {
            var king = board.Pieces.FirstOrDefault(p => p.Row == move.OldRow && p.Col == move.OldCol && p.Type == PieceType.King);
            var rook = board.Pieces.FirstOrDefault(p => p.Row == move.NewRow && p.Col == move.NewCol && p.Type == PieceType.Rook);

            if (king.IsWhite)
            {
                if (!board.CanWhiteSmallCastle && !board.CanWhiteBigCastle)
                {
                    return false;
                }
            }
            else
            {
                if (!board.CanBlackSmallCastle && !board.CanBlackBigCastle)
                {
                    return false;
                }
            }

            int startCol = king.Col;
            int endCol = rook.Col;

            for (int c = startCol; c <= endCol; c++)
            {
                if (board.Pieces.Any(p => p.Row == king.Row && p.Col == c))
                    return false;
            }

            return true;
        }

        private static bool IsMoveBlockedByOwnPiece(ChessBoardModel board, FigureModel currPiece, FigureModel targetPiece)
        {
            if (currPiece.IsWhite == targetPiece.IsWhite)
            {
                return true;
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
            var directions = new List<(int Row, int Col)>
            {
                (isWhite ? 1 : -1, 1),
                (isWhite ? 1 : -1, -1),
                (isWhite ? 1 : -1, 0)
            };

            if ((isWhite && move.OldRow == 1) || (!isWhite && move.OldRow == 6))
            {
                directions.Add((isWhite ? 2 : -2, 0));
            }

            var validSquares = new List<(int Row, int Col)>();
            foreach (var dir in directions)
            {
                var row = move.OldRow + dir.Row;
                var col = move.OldCol + dir.Col;

                if (row < 0 || row > 7 || col < 0 || col > 7) continue;

                var square = board.Pieces.FirstOrDefault(p => p.Row == row && p.Col == col);

                if (square != null)
                {
                    if (dir.Col != 0 && square.IsWhite == isWhite)
                        continue;
                    else if (dir.Col == 0)
                        break;
                }
                else
                {
                    if (dir.Col != 0)
                        continue;
                }

                validSquares.Add((row, col));
            }

            return validSquares.Any(s => s.Row == move.NewRow && s.Col == move.NewCol);
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
