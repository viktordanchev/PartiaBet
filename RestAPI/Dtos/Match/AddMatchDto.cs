using Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace RestAPI.Dtos.Match
{
    public class AddMatchDto
    {
        [Required]
        public GameType GameType { get; set; }

        [Required]
        public decimal BetAmount { get; set; }
    }
}
