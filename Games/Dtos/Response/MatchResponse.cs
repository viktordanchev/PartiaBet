using Core.Enums;

namespace Games.Dtos.Response
{
    public class MatchResponse
    {
        public MatchResponse()
        {
            Players = new List<PlayerResponse>();
        }

        public Guid Id { get; set; }

        public GameType GameId { get; set; }

        public decimal BetAmount { get; set; }

        public int MaxPlayersCount { get; set; }

        public List<PlayerResponse> Players { get; set; }
    }
}
