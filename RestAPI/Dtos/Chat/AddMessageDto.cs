using System.ComponentModel.DataAnnotations;
using Common.Constants;

namespace RestAPI.Dtos.Chat
{
    public class AddMessageDto
    {
        [Required]
        public Guid ReceiverId { get; set; }

        [Required]
        [MaxLength(Validations.Chat.MessageMaxLength)]
        public string Message { get; set; } = string.Empty;
    }
}
