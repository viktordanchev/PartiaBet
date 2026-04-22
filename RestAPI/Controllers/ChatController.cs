using Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace RestAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/chat")]
    public class ChatController : Controller
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost("getHistory")]
        public async Task<IActionResult> GetChatHistory([FromBody] Guid receiverId)
        {
            var userId = Guid.Parse(User.FindFirstValue("Id"));

            var history = await _chatService.GetChatHistoryAsync(userId, receiverId);

            return Ok(history);
        }
    }
}
