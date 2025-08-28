using Core.DTOs.Enums;

namespace Core.DTOs
{
    public class PlayerDto
    {
        public string UserId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
        public int Rating { get; set; }
        public PlayerRole Role { get; set; }
    }
}
