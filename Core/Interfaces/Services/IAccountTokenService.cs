namespace Core.Interfaces.Services
{
    public interface IAccountTokenService
    {
        Task SendVerificationCodeAsync(string email);
    }
}
