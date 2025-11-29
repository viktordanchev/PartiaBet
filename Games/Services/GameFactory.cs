using Common.Exceptions;
using Core.Enums;
using Games.Chess;
using Games.Dtos;
using Games.Dtos.Chess;
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
                GameType.Chess => new ChessService(),
                _ => throw new NotSupportedException($"Game type '{game}' is not supported.")
            };
        }

        public static BaseMakeMoveDto GetMakeMoveDto(GameType game, string jsonData)
        {
            return game switch
            {
                GameType.Chess => JsonSerializer.Deserialize<ChessMakeMoveDto>(
                    jsonData,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }),
                _ => throw new ApiException(InvalidRequest)
            };
        }
    }
}
