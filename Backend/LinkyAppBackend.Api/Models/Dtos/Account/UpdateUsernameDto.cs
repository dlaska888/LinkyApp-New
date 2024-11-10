using System.ComponentModel.DataAnnotations;

namespace LinkyAppBackend.Api.Models.Dtos.Account;

public class UpdateUsernameDto
{
    [Required] public string Username { get; set; } = null!;
}