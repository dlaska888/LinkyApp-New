namespace LinkyAppBackend.Api.Models.Templates;

public class ChangeEmailModel
{
    public string UserName { get; set; } = null!;
    public string NewEmail { get; set; } = null!;
    public string Link { get; set; } = null!;
}