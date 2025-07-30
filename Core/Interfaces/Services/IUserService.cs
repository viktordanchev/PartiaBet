using Core.DTOs.Requests.Account;

namespace Core.Interfaces.Services
{
    public interface IUserService
    {
        Task RegisterUser(RegisterUserRequest data);
    }
}
