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
                return false;

            var currPiece = board.Pieces
                .FirstOrDefault(p => p.Row == move.OldRow && p.Col == move.OldCol);

            if (currPiece == null)
                return false;

            var targetPiece = board.Pieces
                .FirstOrDefault(p => p.Row == move.NewRow && p.Col == move.NewCol);

            if (targetPiece != null)
            {
                if (IsCastleMove(board, currPiece, targetPiece))
                    return CanPerformCastle(board, move);

                if (IsMoveBlockedByOwnPiece(board, currPiece, targetPiece))
                    return false;
            }

            var validator = PieceMoveValidator.GetValidator(currPiece.Type);

            return validator.IsValidMove(board, move);
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
    }
}
