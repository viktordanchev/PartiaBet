using Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configs
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .HasIndex(u => u.Email)
                .IsUnique() 
                .HasDatabaseName("UX_User_Email");

            builder
                .HasIndex(u => u.Username)
                .IsUnique()
                .HasDatabaseName("UX_User_Username");
        }
    }
}
