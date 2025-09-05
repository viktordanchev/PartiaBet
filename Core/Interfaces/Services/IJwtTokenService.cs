using Core.DTOs;

namespace Core.Interfaces.Services
{
    public interface IJwtTokenService
    {
        string GenerateRefreshToken();
        string GenerateAccessToken(UserClaimsDto userClaims);
        void SetRefreshTokenCookie(string refreshToken);
        string ReadRefreshToken(string token);
    }
}
