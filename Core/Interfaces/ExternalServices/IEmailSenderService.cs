namespace Core.Interfaces.ExternalServices
{
    public interface IEmailSenderService
    {
        Task<bool> SendVrfCode(string toEmail, string vrfCode);
        Task<bool> SendPasswordRecoverLink(string toEmail, string token);
    }
}
