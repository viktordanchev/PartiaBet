namespace Core.Models.Match
{
    public class MatchModel
    {
        public MatchModel()
        {
            Players = new List<PlayerModel>();
        }

        public Guid Id { get; set; }

        public decimal BetAmount { get; set; }

        public int MaxPlayersCount { get; set; }

        public List<PlayerModel> Players { get; set; }
    }
}
