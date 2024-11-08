namespace LinkyAppBackend.Api.Models.Dtos.Account;

public class ConfirmEmailChangeDto
{
    public string NewEmail { get; set; } = null!;
    public string Token { get; set; } = null!;
}