using Core.Models.Match;

namespace Games.Chess.Models
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
