namespace Games.Dtos.Response
{
    public class MatchRoomResponse
    {
        public MatchRoomResponse()
        {
            Players = new List<PlayerResponse>();
        }

        public int SpectatorsCount { get; set; }

        public decimal BetAmount { get; set; }

        public List<PlayerResponse> Players { get; set; }
    }
}
