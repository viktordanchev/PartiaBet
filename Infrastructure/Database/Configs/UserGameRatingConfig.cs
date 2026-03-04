using Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configs
{
    public class UserGameRatingConfig : IEntityTypeConfiguration<UserGameRating>
    {
        public void Configure(EntityTypeBuilder<UserGameRating> builder)
        {
            builder
                .HasKey(x => new { x.PlayerId, x.GameType });
        }
    }
}
