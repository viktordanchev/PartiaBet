using Core.Enums;
using Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configs
{
    public class GameConfig : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            builder.HasData(SeedGames());
        }

        private Game[] SeedGames()
        {
            return new Game[]
            {
                new Game
                {
                    Id = GameType.Chess,
                    Name = "Chess",
                    ImgUrl = "https://partiabetstorage.blob.core.windows.net/game-images/chess.jpg"
                },
                new Game
                {
                    Id = GameType.Backgammon,
                    Name = "Backgammon",
                    ImgUrl = "https://partiabetstorage.blob.core.windows.net/game-images/backgammon.png"
                },
                new Game
                {
                    Id = GameType.Belote,
                    Name = "Belote",
                    ImgUrl = "https://partiabetstorage.blob.core.windows.net/game-images/belote.png"
                },
                new Game
                {
                    Id = GameType.SixtySix,
                    Name = "Sixty-Six",
                    ImgUrl = "https://partiabetstorage.blob.core.windows.net/game-images/sixty-six.png"
                }
            };
        }
    }
}
