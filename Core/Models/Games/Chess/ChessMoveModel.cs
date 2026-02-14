namespace Core.Models.Games.Chess
{
    public class ChessMoveModel : GameMoveModel
    {
        public int OldRow { get; set; }

        public int OldCol { get; set; }

        public int NewRow { get; set; }

        public int NewCol { get; set; }
    }
}
