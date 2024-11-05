namespace LinkyAppBackend.Api.Models.Dtos.Group;

public class GetGroupDto : GetDto
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}