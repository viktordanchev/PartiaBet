using Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configs
{
    public class FriendshipConfig : IEntityTypeConfiguration<Friendship>
    {
        public void Configure(EntityTypeBuilder<Friendship> builder)
        {
            builder
                .HasOne(f => f.User)
                .WithMany(u => u.SentFriendRequests)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(f => f.Friend)
                .WithMany(u => u.ReceivedFriendRequests)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
