using Core.DTOs.Requests.Account;
using Core.DTOs.Shared;

namespace Core.Interfaces.Services
{
    public interface IUserService
    {
        Task RegisterUserAsync(RegisterUserRequest data);
        Task<bool> IsUserExistAsync(string email);
        Task UpdatePasswordAsync(string email, string password);
        Task<bool> IsLoginDataValidAsync(LoginRequest request);
        Task<UserClaimsDto> GetClaimsAsync(string email);
    }
}
