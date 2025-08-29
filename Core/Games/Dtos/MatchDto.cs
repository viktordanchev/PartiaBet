namespace Core.Games.Dtos
{
    public class MatchDto
    {
        public MatchDto()
        {
            Players = new List<PlayerDto>();
        }

        public Guid Id { get; set; }

        public decimal BetAmount { get; set; }

        public DateTime DateAndTime { get; set; }

        public int GameId { get; set; }

        public int MaxPlayers { get; set; }

        public int SpectatorsCount { get; set; }

        public IEnumerable<PlayerDto> Players { get; set; }
    }
}
