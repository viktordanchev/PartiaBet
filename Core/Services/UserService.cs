using Common.Exceptions;
using Core.Interfaces.Infrastructure;
using Core.Interfaces.Services;
using Core.Models.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using static Common.Constants.ErrorMessages.Account;

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

        public async Task RegisterUserAsync(RegisterUserModel data)
        {
            var (emailExists, usernameExists) = await _userRepository.IsUserDataUniqueAsync(data.Email, data.Username);

            if (emailExists)
                throw new ApiException(UsedEmail, StatusCodes.Status409Conflict);
            else if (usernameExists)
                throw new ApiException(UsedUsername, StatusCodes.Status409Conflict);

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

        public async Task<bool> IsLoginDataValidAsync(LoginUserModel data)
        {
            var userPasswordHash = await _userRepository.GetUserPasswordHashAsync(data.Email);

            if (userPasswordHash == null)
                return false;

            return _passwordHasher.VerifyHashedPassword(null!, userPasswordHash, data.Password) == PasswordVerificationResult.Success;
        }

        public async Task<UserClaimsModel> GetClaimsAsync(string email)
        {
            return await _userRepository.GetUserClaimsByEmailAsync(email);
        }

        public async Task UpdateUserAsync(UpdateUserModel data, string userEmail)
        {
            var newPassword = string.Empty;
            var profileImageUrl = string.Empty;

            if (!string.IsNullOrEmpty(data.NewPassword))
                newPassword = _passwordHasher.HashPassword(null!, data.NewPassword);

            if (data.ProfileImage != null)
            {
                
            }
        }
    }
}
