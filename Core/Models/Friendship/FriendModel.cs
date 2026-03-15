namespace Core.Models.Friendship
{
    public class FriendModel
    {
        public Guid Id { get; set; }

        public string ProfileImageUrl { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;

        public bool IsOnline { get; set; }
    }
}
