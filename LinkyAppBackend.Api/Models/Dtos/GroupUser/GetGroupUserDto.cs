using LinkyAppBackend.Api.Models.Enums;

namespace LinkyAppBackend.Api.Models.Dtos.GroupUser;

public class GetGroupUserDto : GetDto
{
    public string UserId { get; set; } = null!;
    public string GroupId { get; set; } = null!;
    public GroupRole Role { get; set; }
}