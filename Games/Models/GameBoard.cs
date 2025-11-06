using Games.Chess;
using System.Text.Json.Serialization;

namespace Games.Models
{
    [JsonDerivedType(typeof(ChessBoard), typeDiscriminator: "chess")]
    public abstract class GameBoard
    {
    }
}
