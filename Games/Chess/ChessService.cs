using Core.Games.Enums;
using Core.Interfaces.Games;
using Core.Models.Games;
using Core.Models.Games.Chess;
using Games.Dtos.Chess;

namespace Games.Chess
{
    public class ChessService : IGameService
    {
        public GameBoardModel CreateGameBoard()
        {
            var chessBoard = new ChessBoardModel();

            InitializePieces(chessBoard, true);
            InitializePieces(chessBoard, false);

            return chessBoard;
        }

        public bool IsWinningMove(GameBoardModel board)
        {
            var chessBoard = board as ChessBoardModel;

            var whiteKing = chessBoard.Pieces.FirstOrDefault(p => p.Type == PieceType.King && p.IsWhite);
            var blackKing = chessBoard.Pieces.FirstOrDefault(p => p.Type == PieceType.King && !p.IsWhite);

            return whiteKing == null || blackKing == null;
        }

        public void UpdateBoard(GameBoardModel board, BaseMoveModel move)
        {
            var chessBoard = board as ChessBoardModel;
            var chessMove = move as ChessMoveDto;
            var piece = chessBoard.Pieces.FirstOrDefault(p => p.Row == chessMove.OldRow && p.Col == chessMove.OldCol);

            piece.Row = chessMove.NewRow;
            piece.Col = chessMove.NewCol;
        }

        public void UpdatePlayersInBoard(GameBoardModel board, Guid playerId)
        {
            var chessBoard = board as ChessBoardModel;

            if (chessBoard.WhitePlayerId != Guid.Empty)
            {
                chessBoard.BlackPlayerId = playerId;
            }
            else if (chessBoard.BlackPlayerId != Guid.Empty)
            {
                chessBoard.WhitePlayerId = playerId;
            }
            else 
            {
                if (new Random().Next(0, 2) == 0)
                {
                    chessBoard.WhitePlayerId = playerId;
                }
            }
        }

        public bool IsValidMove(GameBoardModel board, BaseMoveModel move)
        {
            var chessBoard = board as ChessBoardModel;
            var chessMove = move as ChessMoveDto;

            if (chessMove.NewRow < 0 || chessMove.NewRow > 7 || chessMove.NewCol < 0 || chessMove.NewCol > 7)
            {
                return false;
            }

            var piece = chessBoard.Pieces.FirstOrDefault(p => p.Row == chessMove.NewRow && p.Col == chessMove.NewCol);

            //if (piece != null && piece.IsWhite == chessBoard.IsHostWhite)
            //{
            //    return false;
            //}

            bool isValidMove;
            switch (chessMove.PieceType)
            {
                case PieceType.King:
                    isValidMove = IsValidKingMove(chessBoard, chessMove);
                    break;
                case PieceType.Queen:
                    isValidMove = IsValidQueenMove(chessBoard, chessMove);
                    break;
                case PieceType.Rook:
                    isValidMove = IsValidRookMove(chessBoard, chessMove);
                    break;
                default:
                    isValidMove = true;
                    break;
            }

            return isValidMove;
        }

        private bool IsValidKingMove(ChessBoardModel board, ChessMoveDto move)
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

        private bool IsValidQueenMove(ChessBoardModel board, ChessMoveDto move)
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

        private bool IsValidRookMove(ChessBoardModel board, ChessMoveDto move)
        {
            var isValidMove = false;
            var directions = new List<(int Row, int Col)>
            {
                (1, 0),
                (-1, 0),
                (0, 1),
                (0, -1)
            };

            foreach (var dir in directions)
            {
                for (var i = 1; i < 8; i++)
                {
                    var posibleRow = dir.Row + move.OldRow * i;
                    var posibleCol = dir.Col + move.OldCol * i;

                    if (posibleRow == move.NewRow && posibleCol == move.NewCol)
                    {
                        isValidMove = true;
                        break;
                    }
                }
            }

            return isValidMove;
        }

        private bool GetLinearMoves(ChessBoardModel board, ChessMoveDto move, List<(int Row, int Col)> directions)
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

        private bool GetSingleMoves(ChessMoveDto move, List<(int Row, int Col)> directions)
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

        private void InitializePieces(ChessBoardModel board, bool isWhite)
        {
            for (int col = 0; col < 8; col++)
            {
                board.Pieces.Add(new FigureModel() { Type = PieceType.Pawn, Row = isWhite ? 1 : 6, Col = col, IsWhite = isWhite });
            }

            var row = isWhite ? 0 : 7;

            board.Pieces.Add(new FigureModel() { Type = PieceType.Rook, Row = row, Col = 0, IsWhite = isWhite });
            board.Pieces.Add(new FigureModel() { Type = PieceType.Rook, Row = row, Col = 7, IsWhite = isWhite });
            board.Pieces.Add(new FigureModel() { Type = PieceType.Knight, Row = row, Col = 1, IsWhite = isWhite });
            board.Pieces.Add(new FigureModel() { Type = PieceType.Knight, Row = row, Col = 6, IsWhite = isWhite });
            board.Pieces.Add(new FigureModel() { Type = PieceType.Bishop, Row = row, Col = 2, IsWhite = isWhite });
            board.Pieces.Add(new FigureModel() { Type = PieceType.Bishop, Row = row, Col = 5, IsWhite = isWhite });
            board.Pieces.Add(new FigureModel() { Type = PieceType.King, Row = row, Col = 3, IsWhite = isWhite });
            board.Pieces.Add(new FigureModel() { Type = PieceType.Queen, Row = row, Col = 4, IsWhite = isWhite });
        }
    }
}