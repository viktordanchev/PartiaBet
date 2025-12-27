using Core.Models.Match;

namespace Core.Models.Games.Chess
{
    public class ChessBoardModel : GameBoardModel
    {
        public ChessBoardModel()
        {
            Pieces = new List<FigureModel>();
            IsHostWhite = new Random().Next(0, 2) == 0;
        }

        public bool IsHostWhite { get; }

        public List<FigureModel> Pieces { get; set; }
    }
}
