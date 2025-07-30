using Core.DTOs.Requests.Account;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace RestAPI.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserRequest data)
        {
            await _userService.RegisterUserAsync(data);

            return Ok();
        }
    }
}
