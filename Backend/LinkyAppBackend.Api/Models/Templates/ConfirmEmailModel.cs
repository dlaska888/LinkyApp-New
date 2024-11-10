namespace LinkyAppBackend.Api.Models.Templates;

public class ConfirmEmailModel
{
    public string UserName { get; set; } = null!;
    public string Link { get; set; } = null!;
}