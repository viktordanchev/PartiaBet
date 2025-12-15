using Core.Models.User;

namespace Core.Interfaces.Services
{
    public interface IJwtTokenService
    {
        string GenerateRefreshToken();
        string GenerateAccessToken(UserClaimsModel userClaims);
        void SetRefreshTokenCookie(string refreshToken);
        string ReadRefreshToken(string token);
    }
}
