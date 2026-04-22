namespace RestAPI.Dtos.Chat
{
    public class AddMessageDto
    {
        public Guid ReceiverId { get; set; }

        public string Message { get; set; } = string.Empty;
    }
}
