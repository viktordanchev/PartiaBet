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
                    Id = 1,
                    Name = "Chess",
                    MaxPlayers = 2,
                    ImgUrl = "https://partiabetstorage.blob.core.windows.net/game-images/chess.jpg"
                },
                new Game
                {
                    Id = 2,
                    Name = "Backgammon",
                    MaxPlayers = 2,
                    ImgUrl = "https://partiabetstorage.blob.core.windows.net/game-images/backgammon.png"
                },
                new Game
                {
                    Id = 3,
                    Name = "Belote",
                    MaxPlayers = 4,
                    ImgUrl = "https://partiabetstorage.blob.core.windows.net/game-images/belote.png"
                },
                new Game
                {
                    Id = 4,
                    Name = "Sixty-Six",
                    MaxPlayers = 2,
                    ImgUrl = "https://partiabetstorage.blob.core.windows.net/game-images/sixty-six.png"
                }
            };
        }
    }
}
