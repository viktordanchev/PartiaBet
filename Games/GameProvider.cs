using Core.Enums;
using Core.Interfaces.Games;
using Core.Models.Games;
using Games.Backgammon;
using Games.Belote;
using Games.Chess;
using Games.SixtySix;

namespace Games
{
    public class GameProvider : IGameProvider
    {
        private readonly IEnumerable<Game> _games = new List<Game>() 
        { 
            new SixtySixGame(),
            new BackgammonGame(),
            new BeloteGame(),
            new ChessGame()
        };

        public IEnumerable<GameModel> GenerateAllGames()
        {
            var games = _games.Select(g => new GameModel()
            {
                GameType = g.GameType,
                Name = g.Name,
                MaxPlayersCount = g.MaxPlayersCount
            });

            return games;
        }

        public int GetMaxPlayersCount(GameType gameType)
        {
            return _games.FirstOrDefault(g => g.GameType == gameType).MaxPlayersCount;
        }
        
        public bool IsValidGameType(GameType gameType)
        {
            return _games.Any(g => g.GameType == gameType);
        }
    }
}
