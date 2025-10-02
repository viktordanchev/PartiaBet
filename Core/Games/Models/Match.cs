namespace Core.Games.Models
{
    public class Match
    {
        public Match()
        {
            Teams = new List<Team>();
        }

        public Guid Id { get; set; } = Guid.NewGuid();

        public decimal BetAmount { get; set; }

        public DateTime DateAndTime { get; set; }

        public int SpectatorsCount { get; set; }

        public GameBoard Board { get; set; } = null!;

        public List<Team> Teams { get; set; }
    }
}
