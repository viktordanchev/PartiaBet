using Games.Dtos.MatchManagerService;

namespace Games.Dtos.Chess
{
    public class ChessBoardDto : GameBoardDto
    {
        public string WhitePlayerId { get; set; } = null!;
        public List<FigureDto> Pieces { get; set; } = new();
    }
}
