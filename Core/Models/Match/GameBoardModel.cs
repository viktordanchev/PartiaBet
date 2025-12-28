using Core.Models.Games.Chess;
using System.Text.Json.Serialization;

namespace Core.Models.Match
{
    [JsonDerivedType(typeof(ChessBoardModel), "chess")]
    public abstract class GameBoardModel
    {
        public int MaxPlayersCount { get; set; }
    }
}
