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

            _memoryCache.Set(email, token, TimeSpan.FromMinutes(1));
        }

        public async Task SendRecoverPassEmailAsync(string email)
        {
            var token = Guid.NewGuid().ToString();
            await _emailSender.SendPasswordRecoverLink(email, token);

            _memoryCache.Set(email, token, TimeSpan.FromMinutes(10));
        }

        public bool IsTokenValid(string key, string value)
        {
            if (_memoryCache.TryGetValue<string>(key, out var savedValue))
            {
                return savedValue.Equals(value.ToUpper(), StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }
    }
}
