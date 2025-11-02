using Games.Models;

namespace Games.Chess
{
    public class ChessBoard : GameBoard
    {
        public List<int> Pieces { get; set; } = null!;
        public int Total { get; set; }
    }
}
