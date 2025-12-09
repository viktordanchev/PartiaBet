using Games.Chess.Enums;
using Games.Dtos.MatchManagerService;

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
