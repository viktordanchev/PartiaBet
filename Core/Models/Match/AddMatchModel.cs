using Core.Enums;

namespace Core.Models.Match
{
    public class AddMatchModel
    {
        public decimal BetAmount { get; set; }

        public GameType GameType { get; set; }
    }
}
