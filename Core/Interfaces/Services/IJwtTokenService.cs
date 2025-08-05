using Core.DTOs.Shared;

namespace Core.Interfaces.Services
{
    public interface IJwtTokenService
    {
        string GenerateRefreshToken();
        string GenerateAccessToken(UserClaimsDto userClaims);
    }
}
