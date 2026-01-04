using Core.Enums;
using Core.Models.Games;

namespace Core.Models.Match
{
    public class MatchModel
    {
        public MatchModel()
        {
            Players = new List<PlayerModel>();
        }

        public Guid Id { get; set; }

        public GameType GameType { get; set; }

        public decimal BetAmount { get; set; }

        public MatchStatus MatchStatus { get; set; }

        public int MaxPlayersCount { get; set; }

        public GameBoardModel? Board { get; set; }

        public List<PlayerModel> Players { get; set; }
    }
}
