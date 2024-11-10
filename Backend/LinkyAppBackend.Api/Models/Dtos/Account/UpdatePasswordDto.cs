using System.ComponentModel.DataAnnotations;

namespace LinkyAppBackend.Api.Models.Dtos.Account;

public class UpdatePasswordDto
{
    [Required] public string OldPassword { get; set; } = null!;

    [Required] public string NewPassword { get; set; } = null!;

    [Required]
    [Compare("NewPassword", ErrorMessage = "Confirm password must match new password.")]
    public string ConfirmNewPassword { get; set; } = null!;
}