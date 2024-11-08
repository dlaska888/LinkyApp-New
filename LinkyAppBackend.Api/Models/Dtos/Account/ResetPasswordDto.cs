using System.ComponentModel.DataAnnotations;

namespace LinkyAppBackend.Api.Models.Dtos.Account;

public class ResetPasswordDto
{
    public string UserId { get; set; } = null!;
    public string Token { get; set; } = null!;
    public string NewPassword { get; set; } = null!;

    [Compare("Password", ErrorMessage = "The new password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; } = null!;
}