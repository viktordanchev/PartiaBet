namespace Core.Interfaces.Services
{
    public interface IAccountTokenService
    {
        Task SendVerificationCodeAsync(string email);
        Task SendRecoverPassLinkAsync(string email);
        bool IsTokenValid(string key, string value);
    }
}
