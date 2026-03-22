namespace RestAPI.Dtos.Friendship
{
    public class PlayerDataDto
    {
        public PlayerDataDto()
        {
            GamesStats = new List<GameStatsDto>();
        }

        public Guid Id { get; set; }

        public string Username { get; set; } = string.Empty;

        public string ProfileImageUrl { get; set; } = string.Empty;

        public bool IsFriend { get; set; }

        public IEnumerable<GameStatsDto> GamesStats { get; set; }
    }
}
