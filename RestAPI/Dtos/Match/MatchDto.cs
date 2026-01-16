using Core.Enums;
using Core.Models.Games;

namespace RestAPI.Dtos.Match
{
    public class MatchDto
    {
        public MatchDto()
        {
            Players = new List<PlayerDto>();
        }

        public Guid Id { get; set; }

        public decimal BetAmount { get; set; }

        public int MaxPlayersCount { get; set; }

        public MatchStatus Status { get; set; }

        public GameBoardModel Board { get; set; }

        public List<PlayerDto> Players { get; set; }
    }
}
