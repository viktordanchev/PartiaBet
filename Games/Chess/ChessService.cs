using Core.Interfaces.Services;

namespace Games.Chess
{
    public class ChessService : IGamesService
    {
        public ChessConfigs Configs { get; set; } = null!;
    }
}
