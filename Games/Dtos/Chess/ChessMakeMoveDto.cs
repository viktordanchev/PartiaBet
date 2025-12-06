namespace Games.Dtos.Chess
{
    public class ChessMakeMoveDto : BaseMakeMoveDto
    {

        public int OldRow { get; set; }

        public int OldCol { get; set; }

        public int NewRow { get; set; }

        public int NewCol { get; set; }

        public string? PieceType { get; set; }
    }
}
