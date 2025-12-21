using Core.Games.Enums;

namespace Core.Games.Models
{
    public class FigureModel
    {
        public PieceType Type { get; set; }

        public int Row { get; set; }

        public int Col { get; set; }

        public bool IsWhite { get; set; }
    }
}
