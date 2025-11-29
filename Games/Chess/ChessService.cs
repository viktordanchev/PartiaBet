using Games.Chess.Models;
using Games.Chess.Models.Enums;
using Games.Interfaces;
using Games.Models;

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

        private void InitializePieces(ChessBoardModel board, bool isWhite)
        {
            for (int col = 0; col < 8; col++)
            {
                board.Pieces.Add(new FigureModel() { Type = isWhite ? PieceTypes.WhitePawn : Models.Enums.PieceTypes.BlackPawn, Row = isWhite ? 1 : 6, Col = col });
            }

            var row = isWhite ? 0 : 7;

            board.Pieces.Add(new FigureModel() { Type = isWhite ? PieceTypes.WhiteRook : Models.Enums.PieceTypes.BlackRook, Row = row, Col = 0 });
            board.Pieces.Add(new FigureModel() { Type = isWhite ? PieceTypes.WhiteRook : Models.Enums.PieceTypes.BlackRook, Row = row, Col = 7 });
            board.Pieces.Add(new FigureModel() { Type = isWhite ? PieceTypes.WhiteKnight : Models.Enums.PieceTypes.BlackKnight, Row = row, Col = 1 });
            board.Pieces.Add(new FigureModel() { Type = isWhite ? PieceTypes.WhiteKnight : Models.Enums.PieceTypes.BlackKnight, Row = row, Col = 6 });
            board.Pieces.Add(new FigureModel() { Type = isWhite ? PieceTypes.WhiteBishop : Models.Enums.PieceTypes.BlackBishop, Row = row, Col = 2 });
            board.Pieces.Add(new FigureModel() { Type = isWhite ? PieceTypes.WhiteBishop : Models.Enums.PieceTypes.BlackBishop, Row = row, Col = 5 });
            board.Pieces.Add(new FigureModel() { Type = isWhite ? PieceTypes.WhiteKing : Models.Enums.PieceTypes.BlackKing, Row = row, Col = 3 });
            board.Pieces.Add(new FigureModel() { Type = isWhite ? PieceTypes.WhiteQueen : Models.Enums.PieceTypes.BlackQueen, Row = row, Col = 4 });
        }
    }
}
