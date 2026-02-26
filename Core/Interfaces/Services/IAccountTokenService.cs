namespace Core.Interfaces.Services
{
    public interface IAccountTokenService
    {
        Task SendVerificationCodeAsync(string email);
        Task SendRecoverPassEmailAsync(string email);
        bool IsTokenValid(string key, string value);
    }
}
