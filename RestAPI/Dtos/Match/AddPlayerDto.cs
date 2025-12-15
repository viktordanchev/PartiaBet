using System.ComponentModel.DataAnnotations;

namespace RestAPI.Dtos.Match
{
    public class AddPlayerDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string ProfileImageUrl { get; set; } = string.Empty;
    }
}
