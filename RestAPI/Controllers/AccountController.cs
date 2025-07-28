using Core.DTOs.Requests.Account;
using Microsoft.AspNetCore.Mvc;

namespace RestAPI.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : Controller
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserRequest data)
        {
            return Ok();
        }
    }
}
