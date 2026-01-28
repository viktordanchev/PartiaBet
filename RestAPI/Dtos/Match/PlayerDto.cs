namespace RestAPI.Dtos.Match
{
    public class PlayerDto
    {
        public Guid Id { get; set; }

        public string ProfileImageUrl { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;

        public int Rating { get; set; }

        public double TurnTimeLeft { get; set; }

        public bool IsMyTurn { get; set; }
    }
}
