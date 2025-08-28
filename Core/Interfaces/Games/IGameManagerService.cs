using Core.Games.Dtos;

namespace Core.Interfaces.Games
{
    public interface IGameManagerService
    {
        void AddMatchToGame(string game, string matchId);
        void AddUserToMatch(string game, string matchId, User user);
    }
}
