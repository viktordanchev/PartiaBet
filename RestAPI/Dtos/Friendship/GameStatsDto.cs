using Core.Enums;

namespace RestAPI.Dtos.Friendship
{
    public class GameStatsDto
    {
        public GameType GameType { get; set; }

        public int Rating { get; set; }

        public int WinCount { get; set; }

        public int LossCount { get; set; }
    }
}
