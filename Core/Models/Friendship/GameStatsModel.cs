namespace Core.Models.Friendship
{
    public class GameStatsModel
    {
        public string Name { get; set; } = string.Empty;

        public int Rating { get; set; }

        public int WinCount { get; set; }

        public int LossCount { get; set; }
    }
}
