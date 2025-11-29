using Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Games.Dtos.Request
{
    public class CreateMatchRequestDto
    {
        [Required]
        public GameType GameId { get; set; }

        [Required]
        public string DateAndTime { get; set; } = string.Empty;

        [Required]
        public decimal BetAmount { get; set; }
    }
}
