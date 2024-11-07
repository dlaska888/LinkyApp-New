using LinkyAppBackend.Api.Models.Dtos;

namespace LinkyAppBackend.Api.Services.Interfaces;

public interface IAccountService
{
    Task<GetDto> GetAsync(string id);
    Task UpdateEmailAsync(string id, string email);
    Task UpdatePasswordAsync(string id, string password);
    Task UploadProfilePictureAsync(string id, IFormFile file);
}