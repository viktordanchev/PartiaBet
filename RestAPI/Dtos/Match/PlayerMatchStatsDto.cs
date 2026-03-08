namespace RestAPI.Dtos.Match
{
    public class PlayerMatchStatsDto
    {
        public Guid Id { get; set; }

        public string ProfileImageUrl { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;

        public int CurrentRating { get; set; }

        public int NewRating { get; set; }

        public bool IsWinner { get; set; }
    }
}
