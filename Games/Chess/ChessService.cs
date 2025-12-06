using Games.Chess.Constants;
using Games.Chess.Models;
using Games.Dtos;
using Games.Dtos.Chess;
using Games.Interfaces;
using Games.Models;
using System.Data;

namespace Games.Chess
{
    public class ChessService : IGameService
    {
        public ChessService()
        {
            Configs = new ChessConfigs();
        }

        public IGameConfigs Configs { get; }

        public GameBoardModel CreateGameBoard()
        {
            return new ChessBoardModel();
        }

        public void AddToBoard(Guid playerId, GameBoardModel board)
        {
            var chessBoard = board as ChessBoardModel;

            if (chessBoard.Pieces.Count > 0)
            {
                if (string.IsNullOrEmpty(chessBoard.WhitePlayerId))
                {
                    chessBoard.WhitePlayerId = playerId.ToString();
                }

                return;
            }

            var isWhiteTaken = new Random().Next(0, 2) == 0;

            if (isWhiteTaken)
            {
                chessBoard.WhitePlayerId = playerId.ToString();
            }

            InitializePieces(chessBoard, !string.IsNullOrEmpty(chessBoard.WhitePlayerId));
            InitializePieces(chessBoard, string.IsNullOrEmpty(chessBoard.WhitePlayerId));
        }

        public void UpdateBoard(GameBoardModel board, BaseMakeMoveDto move)
        {
            var chessBoard = board as ChessBoardModel;
            var chessMove = move as ChessMakeMoveDto;
            var piece = chessBoard.Pieces.FirstOrDefault(p => p.Row == chessMove.OldRow && p.Col == chessMove.OldCol);

            if (IsValidMove(chessBoard, chessMove))
            {
                piece.Row = chessMove.NewRow;
                piece.Col = chessMove.NewCol;
            }
        }

        public bool IsValidMove(GameBoardModel board, BaseMakeMoveDto move)
        {
            var chessBoard = board as ChessBoardModel;
            var chessMove = move as ChessMakeMoveDto;
            bool isValidMove;

            switch (chessMove.PieceType)
            {
                case PieceTypes.WhiteKing:
                case PieceTypes.BlackKing:
                    isValidMove = IsValidKingMove(chessBoard, chessMove);
                    break;
                default:
                    isValidMove = true;
                    break;
            }

            return isValidMove;
        }

        private bool IsValidKingMove(ChessBoardModel board, ChessMakeMoveDto move)
        {
            var isValidMove = false;
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

            foreach (var dir in directions) 
            {
                var posibleRow = dir.Row + move.NewRow;
                var posibleCol = dir.Col + move.NewCol;

                if (posibleRow == move.OldRow && posibleCol == move.OldCol)
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
                board.Pieces.Add(new FigureModel() { Type = isWhite ? PieceTypes.WhitePawn : PieceTypes.BlackPawn, Row = isWhite ? 1 : 6, Col = col });
            }

            var row = isWhite ? 0 : 7;

            board.Pieces.Add(new FigureModel() { Type = isWhite ? PieceTypes.WhiteRook : PieceTypes.BlackRook, Row = row, Col = 0 });
            board.Pieces.Add(new FigureModel() { Type = isWhite ? PieceTypes.WhiteRook : PieceTypes.BlackRook, Row = row, Col = 7 });
            board.Pieces.Add(new FigureModel() { Type = isWhite ? PieceTypes.WhiteKnight : PieceTypes.BlackKnight, Row = row, Col = 1 });
            board.Pieces.Add(new FigureModel() { Type = isWhite ? PieceTypes.WhiteKnight : PieceTypes.BlackKnight, Row = row, Col = 6 });
            board.Pieces.Add(new FigureModel() { Type = isWhite ? PieceTypes.WhiteBishop : PieceTypes.BlackBishop, Row = row, Col = 2 });
            board.Pieces.Add(new FigureModel() { Type = isWhite ? PieceTypes.WhiteBishop : PieceTypes.BlackBishop, Row = row, Col = 5 });
            board.Pieces.Add(new FigureModel() { Type = isWhite ? PieceTypes.WhiteKing : PieceTypes.BlackKing, Row = row, Col = 3 });
            board.Pieces.Add(new FigureModel() { Type = isWhite ? PieceTypes.WhiteQueen : PieceTypes.BlackQueen, Row = row, Col = 4 });
        }
    }
}
