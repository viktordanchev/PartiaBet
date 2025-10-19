using Core.Enums;
using Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configs
{
    public class GameConfig : IEntityTypeConfiguration<Entities.Game>
    {
        public void Configure(EntityTypeBuilder<Entities.Game> builder)
        {
            builder.HasData(SeedGames());
        }

        private Entities.Game[] SeedGames()
        {
            return new Entities.Game[]
            {
                new Entities.Game
                {
                    Id = Core.Enums.GameType.Chess,
                    Name = "Chess",
                    ImgUrl = "https://partiabetstorage.blob.core.windows.net/game-images/chess.jpg"
                },
                new Entities.Game
                {
                    Id = Core.Enums.GameType.Backgammon,
                    Name = "Backgammon",
                    ImgUrl = "https://partiabetstorage.blob.core.windows.net/game-images/backgammon.png"
                },
                new Entities.Game
                {
                    Id = Core.Enums.GameType.Belote,
                    Name = "Belote",
                    ImgUrl = "https://partiabetstorage.blob.core.windows.net/game-images/belote.png"
                },
                new Entities.Game
                {
                    Id = Core.Enums.GameType.SixtySix,
                    Name = "Sixty-Six",
                    ImgUrl = "https://partiabetstorage.blob.core.windows.net/game-images/sixty-six.png"
                }
            };
        }
    }
}
