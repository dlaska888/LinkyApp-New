namespace LinkyAppBackend.Api.Models.Dtos.Account;

public class GetAccountDto : GetDto
{
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public bool EmailConfirmed { get; set; }
}