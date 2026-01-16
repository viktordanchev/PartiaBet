using Core.Enums;
using Core.Models.Games;

namespace Core.Models.Match
{
    public class MatchModel
    {
        public MatchModel()
        {
            Players = new();
        }

        public Guid Id { get; set; } = Guid.NewGuid();

        public decimal BetAmount { get; set; }

        public GameType GameType { get; set; }

        public int MaxPlayersCount { get; set; }

        public MatchStatus Status { get; set; }

        public Guid CurrentTurnPlayerId { get; set; }

        public List<PlayerModel> Players { get; set; }

        public GameBoardModel? Board { get; set; }
    }
}
