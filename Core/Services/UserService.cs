using Common.Constants;
using Common.Exceptions;
using Core.DTOs.Requests.Account;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<object> _passwordHasher;

        public UserService(IUserRepository userRepository, IPasswordHasher<object> passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task RegisterUserAsync(RegisterUserRequest data)
        {
            var (emailExists, usernameExists) = await _userRepository.IsUserDataUniqueAsync(data.Email, data.Username);

            if (emailExists)
                throw new ApiException(ErrorMessages.UsedEmail, StatusCodes.Status409Conflict);
            else if (usernameExists)
                throw new ApiException(ErrorMessages.UsedUsername, StatusCodes.Status409Conflict);

            data.Password = _passwordHasher.HashPassword(null!, data.Password);

            await _userRepository.AddUserAsync(data);
        }

        public async Task<bool> IsUserExistAsync(string email)
        {
            return await _userRepository.IsUserExistAsync(email);
        }

        public async Task UpdatePasswordAsync(string email, string password)
        {
            password = _passwordHasher.HashPassword(null!, password);

            await _userRepository.UpdatePasswordAsync(email, password);
        }

        public async Task<bool> IsLoginDataValidAsync(LoginRequest request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);

            if (user == null)
                return false;

            return _passwordHasher.VerifyHashedPassword(null!, user.PasswordHash, request.Password) == PasswordVerificationResult.Success;
        }
    }
}
