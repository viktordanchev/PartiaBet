using Core.Models.User;

namespace Core.Interfaces.Services
{
    public interface IJwtTokenService
    {
        string GenerateRefreshToken();
        string GenerateAccessToken(UserClaimsModel userClaims);
        string ReadRefreshToken(string token);
    }
}
