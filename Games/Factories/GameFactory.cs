using Common.Exceptions;
using Core.Enums;
using Core.Interfaces.Games;
using Core.Models.Match;
using Games.Chess;
using Games.Dtos.Chess;
using System.Text.Json;
using System.Text.Json.Serialization;
using static Common.Constants.ErrorMessages;

namespace Games.Factories
{
    public class GameFactory : IGameFactory
    {
        public IGameService GetGameService(GameType game)
        {
            return game switch
            {
                GameType.Chess => new ChessService(),
                _ => throw new NotSupportedException($"Game type '{game}' is not supported.")
            };
        }

        public BaseMoveModel GetMakeMoveDto(GameType game, string jsonData)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));

            return game switch
            {
                GameType.Chess => JsonSerializer.Deserialize<ChessMoveDto>(jsonData, options),
                _ => throw new ApiException(InvalidRequest)
            };
        }
    }
}
