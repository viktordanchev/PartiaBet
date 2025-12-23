using Core.Games.Enums;

namespace Core.DTOs.Responses
{
    public class PieceSkinDto
    {
        public PieceType Type { get; set; }
        public string White { get; set; } = null!;
        public string Black { get; set; } = null!;
    }
}
