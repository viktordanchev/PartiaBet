using Core.Interfaces.ExternalServices;
using Core.Interfaces.Services;
using Microsoft.Extensions.Caching.Memory;

namespace Core.Services
{
    public class AccountTokenService : IAccountTokenService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IEmailSenderService _emailSender;

        public AccountTokenService(IMemoryCache memoryCache, IEmailSenderService emailSender)
        {
            _memoryCache = memoryCache;
            _emailSender = emailSender;
        }

        public async Task SendVerificationCodeAsync(string email)
        {
            var token = Guid.NewGuid().ToString().Substring(0, 6).ToUpper();
            await _emailSender.SendVrfCode(email, token);

            _memoryCache.Set(token.ToLower(), email, TimeSpan.FromMinutes(1));
        }
    }
}
