using Core.Models.Match;

namespace Core.Models.Games.Chess
{
    public class ChessBoardModel : GameBoardModel
    {
        public ChessBoardModel()
        {
            Pieces = new List<FigureModel>();
        }

        public string WhitePlayerId { get; set; } = string.Empty;

        public List<FigureModel> Pieces { get; set; }
    }
}
