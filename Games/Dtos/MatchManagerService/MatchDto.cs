using Core.Enums;

namespace Games.Dtos.MatchManagerService
{
    public class MatchDto
    {
        public MatchDto()
        {
            Players = new List<PlayerDto>();
        }

        public Guid Id { get; set; }

        public GameType GameId { get; set; }

        public decimal BetAmount { get; set; }

        public int MaxPlayersCount { get; set; }

        public List<PlayerDto> Players { get; set; }
    }
}
