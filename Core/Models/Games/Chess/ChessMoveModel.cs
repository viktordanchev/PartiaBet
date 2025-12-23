using Core.Games.Enums;
using Core.Models.Match;

namespace Core.Models.Games.Chess
{
    public class ChessMoveModel : BaseMoveModel
    {
        public int OldRow { get; set; }

        public int OldCol { get; set; }

        public int NewRow { get; set; }

        public int NewCol { get; set; }

        public PieceType PieceType { get; set; }
    }
}
