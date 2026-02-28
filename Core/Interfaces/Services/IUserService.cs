using Core.Models.User;

namespace Core.Interfaces.Services
{
    public interface IUserService
    {
        Task RegisterUserAsync(RegisterUserModel data);
        Task<bool> IsUserExistAsync(string email);
        Task UpdatePasswordAsync(string email, string password);
        Task<bool> IsLoginDataValidAsync(LoginUserModel data);
        Task<UserClaimsModel> GetClaimsAsync(string email);
        Task UpdateUserAsync(UpdateUserModel data, string userEmail);
        Task<UserDataModel?> GetUserDataAsync(string userEmail);
    }
}
