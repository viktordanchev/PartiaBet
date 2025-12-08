using Games.Chess.Enums;

namespace Games.Chess.Models
{
    public class FigureModel
    {
        public PieceType Type { get; set; }

        public int Row { get; set; }

        public int Col { get; set; }

        public bool IsWhite { get; set; }
    }
}
