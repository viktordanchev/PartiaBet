using Core.DTOs;
using Core.Interfaces.Services;
using Core.Services.Configs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Core.Services
{
    /// <summary>
    /// This class is responsible for Jwt token.
    /// </summary>
    public class JwtTokenService : IJwtTokenService
    {
        private JwtTokenConfig _jwtTokenConfig;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JwtTokenService(IOptions<JwtTokenConfig> jwtOptions,
            IHttpContextAccessor httpContextAccessor)
        {
            _jwtTokenConfig = jwtOptions.Value;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Generate JWT refresh token.
        /// </summary>
        public string GenerateRefreshToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtTokenConfig.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            
            var claims = new List<Claim>()
            {
                new Claim("JwtId", Guid.NewGuid().ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: _jwtTokenConfig.Issuer,
                audience: _jwtTokenConfig.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwtTokenConfig.RefreshTokenDays),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Generate JWT access token.
        /// </summary>
        public string GenerateAccessToken(UserClaimsDto userClaims)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtTokenConfig.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>()
            {
                new Claim("Id", userClaims.Id),
                new Claim("Email", userClaims.Email),
                new Claim("Username", userClaims.Username),
                new Claim("ProfileImageUrl", userClaims.Username),
            };

            foreach (var role in userClaims.Roles)
            {
                claims.Add(new Claim("Role", role));
            }

            var token = new JwtSecurityToken(
                issuer: _jwtTokenConfig.Issuer,
                audience: _jwtTokenConfig.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtTokenConfig.AccessTokenMinutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Set refresh token to cookie.
        /// </summary>
        public void SetRefreshTokenCookie(string refreshToken)
        {
            _httpContextAccessor.HttpContext?.Response.Cookies.Append("refreshToken", refreshToken,
                    new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        IsEssential = true,
                        SameSite = SameSiteMode.None,
                        Expires = DateTime.Now.AddDays(_jwtTokenConfig.RefreshTokenDays)
                    });
        }

        public string ReadRefreshToken(string token)
        {
            var jsonToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

            return jsonToken.Claims.First(c => c.Type == ClaimTypes.Email)!.Value;
        }
    }
}
