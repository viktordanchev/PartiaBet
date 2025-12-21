using Core.Games.Models;
using System.Text.Json.Serialization;

namespace Core.Models.Match
{
    [JsonDerivedType(typeof(ChessBoardModel), "chess")]
    public abstract class GameBoardModel
    {
    }
}
