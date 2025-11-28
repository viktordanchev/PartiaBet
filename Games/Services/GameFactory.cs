using Common.Exceptions;
using Core.Enums;
using Games.Chess;
using Games.Chess.Models;
using Games.Dtos;
using Games.Interfaces;
using Games.Models;
using System.Text.Json;
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

        public static GameBoard GetGameBoard(GameType game)
        {
            return game switch
            {
                GameType.Chess => new ChessBoard(),
                _ => throw new ApiException(InvalidRequest)
            };
        }

        public static BaseDto GetDto(GameType game, string jsonData)
        {
            return game switch
            {
                GameType.Chess => JsonSerializer.Deserialize<NewMoveDto>(
                    jsonData,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }),
                _ => throw new ApiException(InvalidRequest)
            };
        }
    }
}
