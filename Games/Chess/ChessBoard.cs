using Games.Chess.Models;
using Games.Chess.Models.Enums;
using Games.Models;

namespace Games.Chess
{
    public class ChessBoard : GameBoard
    {
        private bool isWhiteTaken;

        public ChessBoard()
        {
            Pieces = new List<FigureModel>();
        }

        public string WhitePlayerId { get; set; } = string.Empty;

        public List<FigureModel> Pieces { get; set; }

        public void AddToBoard(Guid playerId)
        {
            isWhiteTaken = new Random().Next(0, 2) == 0 && !isWhiteTaken;

            if (isWhiteTaken)
            {
                WhitePlayerId = playerId.ToString();
                InitializePieces(playerId, true);
            }
            else
            {
                InitializePieces(playerId, false);
            }
        }

        private void InitializePieces(Guid playerId, bool isWhite)
        {
            for (int col = 0; col < 8; col++)
            {
                Pieces.Add(new FigureModel() { PlayerId = playerId, Type = isWhite ? PieceTypes.WhitePawn : Models.Enums.PieceTypes.BlackPawn, Row = isWhite ? 1 : 6, Col = col });
            }

            var row = isWhite ? 0 : 7;

            Pieces.Add(new FigureModel() { PlayerId = playerId, Type = isWhite ? PieceTypes.WhiteRook : Models.Enums.PieceTypes.BlackRook, Row = row, Col = 0 });
            Pieces.Add(new FigureModel() { PlayerId = playerId, Type = isWhite ? PieceTypes.WhiteRook : Models.Enums.PieceTypes.BlackRook, Row = row, Col = 7 });
            Pieces.Add(new FigureModel() { PlayerId = playerId, Type = isWhite ? PieceTypes.WhiteKnight : Models.Enums.PieceTypes.BlackKnight, Row = row, Col = 1 });
            Pieces.Add(new FigureModel() { PlayerId = playerId, Type = isWhite ? PieceTypes.WhiteKnight : Models.Enums.PieceTypes.BlackKnight, Row = row, Col = 6 });
            Pieces.Add(new FigureModel() { PlayerId = playerId, Type = isWhite ? PieceTypes.WhiteBishop : Models.Enums.PieceTypes.BlackBishop, Row = row, Col = 2 });
            Pieces.Add(new FigureModel() { PlayerId = playerId, Type = isWhite ? PieceTypes.WhiteBishop : Models.Enums.PieceTypes.BlackBishop, Row = row, Col = 5 });
            Pieces.Add(new FigureModel() { PlayerId = playerId, Type = isWhite ? PieceTypes.WhiteKing : Models.Enums.PieceTypes.BlackKing, Row = row, Col = 3 });
            Pieces.Add(new FigureModel() { PlayerId = playerId, Type = isWhite ? PieceTypes.WhiteQueen : Models.Enums.PieceTypes.BlackQueen, Row = row, Col = 4 });
        }
    }
}
