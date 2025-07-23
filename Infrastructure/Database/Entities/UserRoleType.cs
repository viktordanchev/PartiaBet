using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Database.Entities
{
    public class UserRoleType
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
