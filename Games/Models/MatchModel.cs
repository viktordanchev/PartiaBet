namespace Games.Models
{
    public class MatchModel
    {
        public MatchModel()
        {
            Players = new List<PlayerModel>();
        }

        public Guid Id { get; } = Guid.NewGuid();

        public decimal BetAmount { get; set; }

        public DateTime DateAndTime { get; set; }

        public int SpectatorsCount { get; set; }

        public GameBoard Board { get; set; } = null!;

        public List<PlayerModel> Players { get; set; }
    }
}
