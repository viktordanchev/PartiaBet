using Core.Models.Games.Chess;
using System.Text.Json.Serialization;

namespace Core.Models.Games
{
    [JsonDerivedType(typeof(ChessBoardModel), "chess")]
    public abstract class GameBoardModel
    {
    }
}
