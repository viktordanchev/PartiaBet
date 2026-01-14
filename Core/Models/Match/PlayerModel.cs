using Core.Enums;

namespace Core.Models.Match
{
    public class PlayerModel
    {
        public Guid Id { get; set; }

        public string ImageUrl { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;

        public int Rating { get; set; }

        public PlayerStatus Status { get; set; }

        public int TurnOrder { get; set; }

        public int TeamNumber { get; set; }
    }
}
