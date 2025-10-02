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
                    Id = 1,
                    Name = "Chess",
                    ImgUrl = "https://partiabetstorage.blob.core.windows.net/game-images/chess.jpg",
                    GameType = GameType.Chess
                },
                new Game
                {
                    Id = 2,
                    Name = "Backgammon",
                    ImgUrl = "https://partiabetstorage.blob.core.windows.net/game-images/backgammon.png",
                    GameType = GameType.Backgammon
                },
                new Game
                {
                    Id = 3,
                    Name = "Belote",
                    ImgUrl = "https://partiabetstorage.blob.core.windows.net/game-images/belote.png",
                    GameType = GameType.Belote
                },
                new Game
                {
                    Id = 4,
                    Name = "Sixty-Six",
                    ImgUrl = "https://partiabetstorage.blob.core.windows.net/game-images/sixty-six.png",
                    GameType = GameType.SixtySix
                }
            };
        }
    }
}
