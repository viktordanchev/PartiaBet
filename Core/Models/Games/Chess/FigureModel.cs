using Core.Games.Enums;

namespace Core.Models.Games.Chess
{
    public class FigureModel
    {
        public PieceType Type { get; set; }

        public int Row { get; set; }

        public int Col { get; set; }

        public bool IsWhite { get; set; }
    }
}
