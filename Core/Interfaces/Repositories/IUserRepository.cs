using Core.DTOs;
using Core.DTOs.Requests.Account;

namespace Core.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task AddUserAsync(RegisterUserRequest data);
        Task<(bool emailExists, bool usernameExists)> IsUserDataUniqueAsync(string email, string username);
        Task<bool> IsUserExistAsync(string email);
        Task UpdatePasswordAsync(string email, string passwordHash);
        Task<string?> GetUserPasswordHashAsync(string email);
        Task<UserClaimsDto> GetUserClaimsByEmailAsync(string email);
    }
}
