using Common.Exceptions;
using Core.Enums;
using Games.Interfaces;
using static Common.Constants.ErrorMessages;

namespace Games.Services
{
    public class GameFactory
    {
        public static IGameService GetGameService(GameType game)
        {
            return game switch
            {
                GameType.Chess => new Chess.ChessService(),
                _ => throw new NotSupportedException($"Game type '{game}' is not supported.")
            };
        }

        public static IGameConfigs GetGameConfigs(GameType game)
        {
            return game switch
            {
                GameType.Chess => new Chess.ChessConfigs(),
                _ => throw new ApiException(InvalidRequest)
            };
        }
    }
}
