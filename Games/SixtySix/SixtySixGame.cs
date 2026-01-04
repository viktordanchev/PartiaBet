using Core.Enums;

namespace Games.SixtySix
{
    public class SixtySixGame : Game
    {
        public SixtySixGame()
        {
            GameType = GameType.SixtySix;
            Name = "Sixty-Six";
            ImgUrl = "https://partiabetstorage.blob.core.windows.net/game-images/sixty-six.png";
            MaxPlayersCount = 2;
        }
    }
}
