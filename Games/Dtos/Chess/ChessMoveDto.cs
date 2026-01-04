using Core.Games.Enums;
using Core.Models.Games;

namespace Games.Dtos.Chess
{
    public class ChessMoveDto : BaseMoveModel
    {
        public int OldRow { get; set; }

        public int OldCol { get; set; }

        public int NewRow { get; set; }

        public int NewCol { get; set; }

        public PieceType PieceType { get; set; }
    }
}
