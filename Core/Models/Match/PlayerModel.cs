using Core.Enums;

namespace Core.Models.Match
{
    public class PlayerModel
    {
        public PlayerModel()
        {
            Timer = new PlayerTimer();
        }

        public Guid Id { get; set; }

        public string ProfileImageUrl { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;

        public int Rating { get; set; }

        public PlayerTimer Timer { get; set; }

        public bool IsOnTurn { get; set; }

        public PlayerStatus Status { get; set; }

        public int TurnOrder { get; set; }

        public int TeamNumber { get; set; }
    }
}
