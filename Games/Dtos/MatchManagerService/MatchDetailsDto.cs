using Core.Enums;

namespace Games.Dtos.MatchManagerService
{
    public class MatchDetailsDto
    {
        public MatchDetailsDto()
        {
            Players = new List<PlayerDto>();
        }

        public GameType Game {  get; set; }

        public int SpectatorsCount { get; set; }

        public decimal BetAmount { get; set; }

        public List<PlayerDto> Players { get; set; }

        public GameBoardDto Board { get; set; } = null!;
    }
}
