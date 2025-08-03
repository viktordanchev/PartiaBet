using Core.DTOs.Requests.Account;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using static Common.Constants.ErrorMessages;
using static Common.Constants.Messages;

namespace RestAPI.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IAccountTokenService _accountTokenService;

        public AccountController(IUserService userService,
            IAccountTokenService accountTokenService)
        {
            _userService = userService;
            _accountTokenService = accountTokenService;
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

            await _accountTokenService.SendVerificationCodeAsync(email);

            return Ok(new { Message = NewVrfCode });
        }
    }
}
