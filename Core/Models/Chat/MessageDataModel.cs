namespace Core.Models.Chat
{
    public class MessageDataModel
    {
        public Guid Id { get; set; }
        public Guid SenderId { get; set; }
        public DateTime DateAndTime { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
