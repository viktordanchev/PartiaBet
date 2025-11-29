using Games.Chess.Models;
using System.Text.Json.Serialization;

namespace Games.Models
{
    [JsonDerivedType(typeof(ChessBoardModel), typeDiscriminator: "chess")]
    public abstract class GameBoardModel
    {
    }
}
