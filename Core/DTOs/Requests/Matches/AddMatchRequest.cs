using Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.Requests.Matches
{
    public class AddMatchRequest
    {
        [Required]
        public GameType GameId { get; set; }

        [Required]
        public decimal BetAmount { get; set; }

        [Required]
        public string DateAndTime { get; set; } = string.Empty;
    }
}
