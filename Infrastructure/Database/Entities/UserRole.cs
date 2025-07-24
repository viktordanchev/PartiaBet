using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Database.Entities
{
    public class UserRole
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;

        [Required]
        public Guid RoleTypeId { get; set; }

        [ForeignKey(nameof(RoleTypeId))]
        public UserRoleType RoleType { get; set; } = null!;
    }
}
