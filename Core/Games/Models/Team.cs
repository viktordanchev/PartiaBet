namespace Core.Games.Models
{
    public class Team
    {
        public Team()
        {
            Players = new List<Player>();
        }

        public Guid Id { get; set; } = Guid.NewGuid();

        public List<Player> Players { get; set; }
    }
}
