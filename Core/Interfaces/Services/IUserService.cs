using Core.DTOs.Requests.Account;

namespace Core.Interfaces.Services
{
    public interface IUserService
    {
        Task RegisterUserAsync(RegisterUserRequest data);
    }
}
