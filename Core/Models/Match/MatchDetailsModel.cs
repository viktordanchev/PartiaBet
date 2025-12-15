namespace Core.Models.Match
{
    public class MatchDetailsModel
    {
        public MatchDetailsModel()
        {
            Players = new List<PlayerModel>();
        }

        public decimal BetAmount { get; set; }

        public int MaxPlayersCount { get; set; }

        public GameBoardModel? Board { get; set; }

        public List<PlayerModel> Players { get; set; }
    }
}
