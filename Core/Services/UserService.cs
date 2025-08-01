using Common.Constants;
using Common.Exceptions;
using Core.DTOs.Requests.Account;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;

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
            var (emailExists, usernameExists) = await _userRepository.IsUserDataUniqueAsync(data.Email, data.Username);

            if (emailExists)
                throw new ApiException(ErrorMessages.RegisteredEmail, StatusCodes.Status409Conflict);
            else if (usernameExists)
                throw new ApiException(ErrorMessages.UsedUsername, StatusCodes.Status409Conflict);

            await _userRepository.AddUserAsync(data);
        }
    }
}
