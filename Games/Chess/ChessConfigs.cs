using Games.Interfaces;

namespace Games.Chess
{
    public class ChessConfigs : IGameConfigs
    {
        public int TeamSize { get; set; } = 1;
        public int TeamsCount { get; set; } = 2;
    }
}
