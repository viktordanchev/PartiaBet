using Core.Games.Enums;

namespace Games.Dtos.Chess
{
    public class FigureDto
    {
        public PieceType Type { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }
        public bool IsWhite { get; set; }
    }
}
