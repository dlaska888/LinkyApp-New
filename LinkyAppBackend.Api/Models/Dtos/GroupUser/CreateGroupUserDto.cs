using System.ComponentModel.DataAnnotations;
using LinkyAppBackend.Api.Models.Enums;

namespace LinkyAppBackend.Api.Models.Dtos.GroupUser;

public class CreateGroupUserDto
{
    [EmailAddress]
    public string Email { get; set; } = null!;
    public GroupRole Role { get; set; }
}