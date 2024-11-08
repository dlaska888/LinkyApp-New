using LinkyAppBackend.Api.Models.Dtos.Account;

namespace LinkyAppBackend.Api.Services;

public interface IAccountService
{
    Task<GetAccountDto> GetAsync();
    Task UpdateUsernameAsync(UpdateUsernameDto dto);
    Task UpdatePasswordAsync(UpdatePasswordDto dto);
    Task UploadProfilePictureAsync(IFormFile file);
    Task DeleteProfilePictureAsync();
    Task RequestEmailConfirmationAsync(RequestEmailConfirmationDto dto);
    Task ConfirmEmailAsync(ConfirmEmailDto dto);
    Task RequestEmailChangeAsync(RequestEmailChangeDto dto);
    Task ConfirmEmailChangeAsync(ConfirmEmailChangeDto dto);
    Task RequestPasswordResetAsync(RequestPasswordResetDto dto);
    Task ResetPasswordAsync(ResetPasswordDto dto);
}