using Core.Models.Match;

namespace Core.Models.Games.Chess
{
    public class ChessBoardModel : GameBoardModel
    {
        public ChessBoardModel()
        {
            Pieces = new List<FigureModel>();
        }

        public Guid WhitePlayerId { get; set; }

        public List<FigureModel> Pieces { get; set; }
    }
}
