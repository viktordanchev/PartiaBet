using AutoMapper;
using Core.Interfaces.Services;
using Core.Models.User;
using Microsoft.AspNetCore.Mvc;
using RestAPI.Dtos.User;
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
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IHostEnvironment _environment;
        private readonly IMapper _mapper;

        public AccountController(IUserService userService,
            IAccountTokenService accountTokenService,
            IJwtTokenService jwtTokenService,
            IHostEnvironment environment,
            IMapper mapper)
        {
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

        //[HttpPost("recoverPass")]
        //public async Task<IActionResult> RecoverPassword(RecoverPasswordDto data)
        //{
        //    if (!await _userService.IsUserExistAsync(data.Email))
        //    {
        //        return BadRequest(new { Error = NotRegistered });
        //    }
        //    else if (_accountTokenService.IsTokenValid(data.Email, data.Token))
        //    {
        //        return BadRequest(new { Error = InvalidToken });
        //    }
        //
        //    await _userService.UpdatePasswordAsync(data.Email, data.Password);
        //
        //    return Ok();
        //}

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
