using Microsoft.AspNetCore.Http;

namespace Core.Interfaces.ExternalServices
{
    public interface IBlobStorageService
    {
        Task<string> UploadProfileImageAsync(IFormFile imageFile, string userEmail);
        Task DeleteProfileImageAsync(string imageUrl, string container);
    }
}