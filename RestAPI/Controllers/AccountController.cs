using AutoMapper;
using Core.Interfaces.Service;
using Core.Interfaces.Services;
using Core.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestAPI.Dtos.User;
using System.Security.Claims;
using static Common.Constants.ErrorMessages.Account;
using static Common.Constants.Messages.Account;

namespace RestAPI.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : Controller
    {
        private readonly IMemoryCacheService _memoryCacheService;
        private readonly IUserService _userService;
        private readonly IAccountTokenService _accountTokenService;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IHostEnvironment _environment;
        private readonly IMapper _mapper;

        public AccountController(IMemoryCacheService memoryCacheService,
            IUserService userService,
            IAccountTokenService accountTokenService,
            IJwtTokenService jwtTokenService,
            IHostEnvironment environment,
            IMapper mapper)
        {
            _memoryCacheService = memoryCacheService;
            _userService = userService;
            _accountTokenService = accountTokenService;
            _jwtTokenService = jwtTokenService;
            _environment = environment;
            _mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDto data)
        {
            var dataModel = _mapper.Map<LoginUserModel>(data);

            if (!await _userService.IsLoginDataValidAsync(dataModel))
                return BadRequest(new { Error = InvalidLoginData });

            var userClaims = await _userService.GetClaimsAsync(data.Email);
            var accessToken = _jwtTokenService.GenerateAccessToken(userClaims);

            if (data.RememberMe)
            {
                var refreshToken = _jwtTokenService.GenerateRefreshToken();
                _jwtTokenService.SetRefreshTokenCookie(refreshToken);
            }

            return Ok(new { Token = accessToken });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDto data)
        {
            var dataModel = _mapper.Map<RegisterUserModel>(data);

            if (_environment.IsProduction() && !_accountTokenService.IsTokenValid(data.Email, data.VrfCode))
                return BadRequest(new { Error = InvalidVrfCode });
            
            await _userService.RegisterUserAsync(dataModel);

            return NoContent();
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

        [HttpPost("sendRecoverPasswordEmail")]
        public async Task<IActionResult> SendRecoverPasswordEmail([FromBody] string email)
        {
            if (!await _userService.IsUserExistAsync(email))
            {
                return BadRequest(new { Error = NotRegistered });
            }

            await _accountTokenService.SendRecoverPassEmailAsync(email);

            return Ok(new { Message = SendedPassRecoverEmail });
        }

        [HttpPost("recoverPassword")]
        public async Task<IActionResult> RecoverPassword([FromBody] RecoverPasswordDto data)
        {
            if (!_memoryCacheService.isExist(data.Token))
                return BadRequest(new { Error = InvalidToken });

            var userEmail = _memoryCacheService.GetValue(data.Token);
            await _userService.UpdatePasswordAsync(userEmail, data.Password);

            return NoContent();
        }

        [HttpPut("updateUser")]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto data)
        {
            if (!string.IsNullOrEmpty(data.CurrentPassword) && data.CurrentPassword == data.NewPassword)
                return BadRequest(new { Error = SamePassword });

            var userEmail = User.FindFirstValue("Email");
            var dataModel = _mapper.Map<UpdateUserModel>(data);

            await _userService.UpdateUserAsync(dataModel, userEmail);

            var userClaims = await _userService.GetClaimsAsync(userEmail);
            var accessToken = _jwtTokenService.GenerateAccessToken(userClaims);

            return Ok(
                new
                {
                    Message = UserDataUpdated,
                    Token = accessToken
                });
        }

        [HttpGet("refreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var newAccessToken = string.Empty;

            if (refreshToken != null)
            {
                var userEmail = _jwtTokenService.ReadRefreshToken(refreshToken);
                var userClaims = await _userService.GetClaimsAsync(userEmail);

                newAccessToken = _jwtTokenService.GenerateAccessToken(userClaims);
            }

            return Ok(new { Token = newAccessToken });
        }
    }
}
