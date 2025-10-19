using System.ComponentModel.DataAnnotations;

namespace Games.Dtos.Request.Matches
{
    public class GetMatchDataRequestDto
    {
        [Required]
        public string Game { get; set; } = string.Empty;

        [Required]
        public Guid MatchId { get; set; }
    }
}
