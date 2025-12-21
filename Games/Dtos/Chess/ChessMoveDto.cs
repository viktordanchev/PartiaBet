using Core.Models.Match;
using Games.Chess.Enums;

namespace Games.Dtos.Chess
{
    public class ChessMoveDto : BaseMoveDto
    {
        public int OldRow { get; set; }

        public int OldCol { get; set; }

        public int NewRow { get; set; }

        public int NewCol { get; set; }

        public PieceType PieceType { get; set; }
    }
}
