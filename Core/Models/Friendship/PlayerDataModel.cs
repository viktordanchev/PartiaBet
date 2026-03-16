namespace Core.Models.Friendship
{
    public class PlayerDataModel
    {
        public PlayerDataModel()
        {
            GamesStats = new List<GameStatsModel>();
        }

        public Guid Id { get; set; }

        public string Username { get; set; } = string.Empty;

        public string ProfileImageUrl { get; set; } = string.Empty;

        public bool IsFriend { get; set; }

        public IEnumerable<GameStatsModel> GamesStats { get; set; }
    }
}
