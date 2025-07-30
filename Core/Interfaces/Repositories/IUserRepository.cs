using Core.DTOs.Requests.Account;

namespace Core.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task AddUserAsync(RegisterUserRequest data);
    }
}
