using Core.Games.Enums;
using Core.Models.Games.Chess;
using Games.Chess.PieceMovement;

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
                    return CanPerformCastle(board, currPiece);

                if (IsMoveBlockedByOwnPiece(board, currPiece, targetPiece))
                    return false;
            }

            var validator = PieceMoveValidator.GetValidator(currPiece.Type);

            return validator.IsValidMove(board, move);
        }

        public static bool IsWinningMove(ChessBoardModel board, ChessMoveModel move)
        {
            var attackerPiece = board.Pieces
                .FirstOrDefault(p => p.Row == move.NewRow && p.Col == move.NewCol);

            var kingPiece = board.Pieces.FirstOrDefault(p => p.Type == PieceType.King && p.IsWhite != attackerPiece.IsWhite);

            if (!ChessAttackDetector.IsSquareAttacked(board, kingPiece.Row, kingPiece.Col, kingPiece.IsWhite))
                return false;

            if (CanPerformCastle(board, kingPiece))
                return false;

            foreach (var direction in Directions.King)
            {
                var newKingRow = kingPiece.Row + direction.Row;
                var newKingCol = kingPiece.Col + direction.Col;

                if (newKingRow < 0 || newKingRow > 7 || newKingCol < 0 || newKingCol > 7)
                    continue;

                var piece = board.Pieces.FirstOrDefault(p => p.Row == newKingRow && p.Col == newKingCol);

                if (piece != null && piece.IsWhite == kingPiece.IsWhite)
                    continue;

                if (!ChessAttackDetector.IsSquareAttacked(board, newKingRow, newKingCol, kingPiece.IsWhite))
                    return false;
            }

            return true;
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

        private static bool CanPerformCastle(ChessBoardModel board, FigureModel king)
        {
            if (king.IsWhite)
            {
                if (!board.CanWhiteSmallCastle && !board.CanWhiteBigCastle)
                    return false;
            }
            else
            {
                if (!board.CanBlackSmallCastle && !board.CanBlackBigCastle)
                    return false;
            }

            int row = king.Row;

            var kingsideRook = board.Pieces.FirstOrDefault(p => p.Row == row && p.Col == 7 && p.Type == PieceType.Rook && p.IsWhite == king.IsWhite);
            if (kingsideRook != null)
            {
                bool pathClear = true;
                for (int c = king.Col + 1; c < 7; c++)
                {
                    if (board.Pieces.Any(p => p.Row == row && p.Col == c))
                    {
                        pathClear = false;
                        break;
                    }
                }

                if (pathClear)
                    return true;
            }

            var queensideRook = board.Pieces.FirstOrDefault(p => p.Row == row && p.Col == 0 && p.Type == PieceType.Rook && p.IsWhite == king.IsWhite);
            if (queensideRook != null)
            {
                bool pathClear = true;
                for (int c = 1; c < king.Col; c++)
                {
                    if (board.Pieces.Any(p => p.Row == row && p.Col == c))
                    {
                        pathClear = false;
                        break;
                    }
                }

                if (pathClear)
                    return true;
            }

            return false;
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
