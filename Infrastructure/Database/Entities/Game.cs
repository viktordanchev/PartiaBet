using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Database.Entities
{
    public class Game
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
