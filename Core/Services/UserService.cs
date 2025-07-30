using Core.DTOs.Requests.Account;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;

namespace Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task RegisterUserAsync(RegisterUserRequest data)
        {
            await _userRepository.AddUserAsync(data);
        }
    }
}
