using Core.Dtos.Account;
using Core.DTOs;

namespace Core.Interfaces.Services
{
    public interface IUserService
    {
        Task RegisterUserAsync(RegisterUserDto data);
        Task<bool> IsUserExistAsync(string email);
        Task UpdatePasswordAsync(string email, string password);
        Task<bool> IsLoginDataValidAsync(LoginDto request);
        Task<UserClaimsDto> GetClaimsAsync(string email);
    }
}
