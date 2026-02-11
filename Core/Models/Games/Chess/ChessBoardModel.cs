namespace Core.Models.Games.Chess
{
    public class ChessBoardModel : GameBoardModel
    {
        public ChessBoardModel()
        {
            Pieces = new List<FigureModel>();
        }

        public Guid WhitePlayerId { get; set; }

        public Guid BlackPlayerId { get; set; }

        public bool CanWhiteSmallCastle { get; set; }
        public bool CanWhiteBigCastle { get; set; }

        public bool CanBlackSmallCastle { get; set; }
        public bool CanBlackBigCastle { get; set; }

        public List<FigureModel> Pieces { get; set; }
    }
}
