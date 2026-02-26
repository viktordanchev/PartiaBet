using Microsoft.AspNetCore.Http;

namespace Core.Interfaces.ExternalServices
{
    public interface IBlobStorageService
    {
        Task<string> UploadProfileImageAsync(IFormFile file, string userId);
        Task DeleteProfileImageAsync(string imageUrl, string container);
    }
}