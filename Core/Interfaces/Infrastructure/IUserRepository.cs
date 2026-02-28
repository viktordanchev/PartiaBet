using Core.Models.User;

namespace Core.Interfaces.Infrastructure
{
    public interface IUserRepository
    {
        Task AddUserAsync(RegisterUserModel data);
        Task<(bool emailExists, bool usernameExists)> IsUserDataUniqueAsync(string email, string username);
        Task<bool> IsUserExistAsync(string email);
        Task UpdatePasswordAsync(string email, string passwordHash);
        Task<string?> GetUserPasswordHashAsync(string email);
        Task<UserClaimsModel> GetUserClaimsByEmailAsync(string email);
        Task<UserDataModel?> GetUserDataAsync(string userEmail);
        Task UpdateUserDataAsync(UpdateUserModel data, string userEmail);
    }
}
