using System.ComponentModel.DataAnnotations;

namespace RestAPI.Dtos.Match
{
    public class AddMatchDto
    {
        [Required]
        public int GameId { get; set; }

        [Required]
        public string DateAndTime { get; set; } = string.Empty;

        [Required]
        public decimal BetAmount { get; set; }
    }
}
