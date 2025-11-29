using Core.Enums;
using Games.Models;

namespace Games.Dtos.Response
{
    public class MatchRoomResponse
    {
        public MatchRoomResponse()
        {
            Players = new List<PlayerResponse>();
        }

        public GameType Game {  get; set; }

        public int SpectatorsCount { get; set; }

        public decimal BetAmount { get; set; }

        public List<PlayerResponse> Players { get; set; }

        public GameBoardModel Board { get; set; } = null!;
    }
}
