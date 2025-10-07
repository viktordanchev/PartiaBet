using Core.Enums;
using Core.Games.Models;

namespace Core.Games.Dtos
{
    public class MatchDto
    {
        public MatchDto()
        {
            Teams = new List<TeamDto>();
        }

        public Guid Id { get; set; }

        public GameType GameType { get; set; }

        public string DateAndTime { get; set; } = string.Empty;

        public decimal BetAmount { get; set; }

        public int TeamsCount { get; set; }

        public int TeamSize { get; set; }

        public int SpectatorsCount { get; set; }

        public GameBoard Board { get; set; } = null!;

        public List<TeamDto> Teams { get; set; }
    }
}
