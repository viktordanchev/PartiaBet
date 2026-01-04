using Core.Enums;

namespace Games.Belote
{
    public class BeloteGame : Game
    {
        public BeloteGame()
        {
            GameType = GameType.Belote;
            Name = "Belote";
            ImgUrl = "https://partiabetstorage.blob.core.windows.net/game-images/belote.png";
            MaxPlayersCount = 4;
        }
    }
}
