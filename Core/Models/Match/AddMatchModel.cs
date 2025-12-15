namespace Core.Models.Match
{
    public class AddMatchModel
    {
        public decimal BetAmount { get; set; }
        public string DateAndTime { get; set; } = string.Empty;
        public int GameId { get; set; }
    }
}
