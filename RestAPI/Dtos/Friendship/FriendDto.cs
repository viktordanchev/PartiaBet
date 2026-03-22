namespace RestAPI.Dtos.Friendship
{
    public class FriendDto
    {
        public Guid Id { get; set; }

        public string ProfileImageUrl { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;

        public bool IsOnline { get; set; }
    }
}
