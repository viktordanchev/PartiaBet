using Core.DTOs.Requests.Account;
using Core.Interfaces.Services;
using Core.Interfaces.ExternalServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using static Common.Constants.ErrorMessages;
using static Common.Constants.Messages;

namespace RestAPI.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMemoryCache _memoryCache;
        private readonly IEmailSenderService _emailSender;

        public AccountController(IUserService userService, 
            IMemoryCache memoryCache,
            IEmailSenderService emailSender)
        {
            _userService = userService;
            _memoryCache = memoryCache;
            _emailSender = emailSender;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserRequest data)
        {
            await _userService.RegisterUserAsync(data);

            return Ok();
        }

        [HttpPost("sendVrfCode")]
        public async Task<IActionResult> SendVerificationCode([FromBody] string email)
        {
            if (await _userService.IsUserExistAsync(email))
            {
                return BadRequest(new { Error = UsedEmail });
            }

            var token = Guid.NewGuid().ToString().Substring(0, 6).ToUpper();
            var isSended = await _emailSender.SendVrfCode(email, token);

            _memoryCache.Set(token.ToLower(), email, TimeSpan.FromMinutes(1));

            return Ok(new { Message = NewVrfCode });
        }
    }
}
