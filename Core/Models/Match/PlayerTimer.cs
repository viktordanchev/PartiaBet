namespace Core.Models.Match
{
    public class PlayerTimer
    {
        public DateTime TurnExpiresAt { get; set; }

        public TimeSpan TimeLeft { get; set; }
    }
}
