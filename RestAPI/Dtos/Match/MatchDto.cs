namespace RestAPI.Dtos.Match
{
    public class MatchDto
    {
        public MatchDto()
        {
            Players = new List<PlayerDto>();
        }

        public decimal BetAmount { get; set; }

        public int MaxPlayersCount { get; set; }

        public List<PlayerDto> Players { get; set; }
    }
}
