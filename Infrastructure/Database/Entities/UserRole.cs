using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Database.Entities
{
    public class UserRole
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;

        [Required]
        public int RoleTypeId { get; set; }

        [ForeignKey(nameof(RoleTypeId))]
        public UserRoleType RoleType { get; set; } = null!;
    }
}
