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

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest data)
        {
            if (!await _userService.IsLoginDataValidAsync(data))
                return BadRequest(new { Error = InvalidLoginData });

            var userClaims = await _userService.GetUserClaimsAsync(request.Email);
            var accessToken = _jwtTokenService.GenerateAccessToken(userClaims);

            if (request.RememberMe)
            {
                var refreshToken = _jwtTokenService.GenerateRefreshToken();

                Response.Cookies.Append("refreshToken", refreshToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    IsEssential = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.Now.AddMonths(int.Parse(_configs["Cookies:RefreshJWTTokenMonths"])),
                });
            }

            return Ok(new { Token = accessToken });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserRequest data)
        {
            if(_accountTokenService.IsTokenValid(data.Email, data.VerificationCode))
                return BadRequest(new { Error = InvalidVrfCode });

            await _userService.RegisterUserAsync(data);

            return Ok();
        }

        [HttpPost("sendVrfCode")]
        public async Task<IActionResult> SendVerificationCode([FromBody] string email)
        {
            if (!await _userService.IsUserExistAsync(email))
            {
                return BadRequest(new { Error = NotRegistered });
            }

            await _accountTokenService.SendVerificationCodeAsync(email);

            return Ok(new { Message = NewVrfCode });
        }

        [HttpPost("sendRecoverPassLink")]
        public async Task<IActionResult> SendRecoverPasswordLink([FromBody] string email)
        {
            if (!await _userService.IsUserExistAsync(email))
            {
                return BadRequest(new { Error = NotRegistered });
            }

            await _accountTokenService.SendRecoverPassLinkAsync(email);

            return Ok(new { Message = SendedPassRecoverLink });
        }

        [HttpPost("recoverPass")]
        public async Task<IActionResult> RecoverPassword(RecoverPasswordRequest data)
        {
            if (!await _userService.IsUserExistAsync(data.Email))
            {
                return BadRequest(new { Error = NotRegistered });
            }
            else if(_accountTokenService.IsTokenValid(data.Email, data.Token))
            {
                return BadRequest(new { Error = InvalidToken });
            }
            
            await _userService.UpdatePasswordAsync(data.Email, data.Password);

            return Ok();
        }
    }
}
