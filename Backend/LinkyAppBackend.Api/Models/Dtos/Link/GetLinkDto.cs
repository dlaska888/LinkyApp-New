namespace LinkyAppBackend.Api.Models.Dtos.Link;

public class GetLinkDto : GetDto
{
    public string Title { get; set; } = null!;
    public string Url { get; set; } = null!;
    public string CreatorId { get; set; } = null!;
}