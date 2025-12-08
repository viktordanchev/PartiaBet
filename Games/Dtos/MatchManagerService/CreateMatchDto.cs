using Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Games.Dtos.MatchManagerService
{
    public class CreateMatchDto
    {
        [Required]
        public GameType GameId { get; set; }

        [Required]
        public string DateAndTime { get; set; } = string.Empty;

        [Required]
        public decimal BetAmount { get; set; }
    }
}
