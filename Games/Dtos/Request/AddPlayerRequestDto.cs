using System.ComponentModel.DataAnnotations;

namespace Games.Dtos.Request
{
    public class AddPlayerRequestDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string ProfileImageUrl { get; set; } = string.Empty;
    }
}
