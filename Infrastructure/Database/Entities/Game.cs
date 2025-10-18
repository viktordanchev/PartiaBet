using Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Database.Entities
{
    public class Game
    {
        [Key]
        public GameType Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string ImgUrl { get; set; } = string.Empty;
    }
}
