using Core.DTOs.Requests.Account;

namespace Core.Interfaces.Services
{
    public interface IUserService
    {
        Task RegisterUserAsync(RegisterUserRequest data);
        Task<bool> IsUserExistAsync(string email);
        Task UpdatePasswordAsync(string email, string password);
    }
}
