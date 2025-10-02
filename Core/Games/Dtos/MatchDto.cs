using Core.Enums;

namespace Core.Games.Dtos
{
    public class MatchDto
    {
        public MatchDto()
        {
            Players = new List<PlayerDto>();
        }

        public Guid Id { get; set; }

        public GameType GameType { get; set; }

        public decimal BetAmount { get; set; }

        public string DateAndTime { get; set; } = string.Empty;

        public int GameId { get; set; }

        public int MaxPlayers { get; set; }

        public int SpectatorsCount { get; set; }

        public List<PlayerDto> Players { get; set; }
    }
}
