using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Database.Entities
{
    public class UserRole
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(RoleType))]
        public Guid RoleTypeId { get; set; }
        public UserRoleType RoleType { get; set; } = null!;
    }
}
