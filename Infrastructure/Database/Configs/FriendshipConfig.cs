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
                .HasKey(f => new { f.FirstUserId, f.SecondUserId });

            builder
                .HasOne(f => f.FirstUser)
                .WithMany(u => u.SentFriendRequests)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(f => f.SecondUser)
                .WithMany(u => u.ReceivedFriendRequests)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
