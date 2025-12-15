using Games.Dtos.Chess;
using System.Text.Json.Serialization;

namespace Games.Dtos.MatchManagerService
{
    [JsonDerivedType(typeof(ChessBoardDto))]
    public abstract class GameBoardDto
    {
    }
}
