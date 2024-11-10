using System.ComponentModel.DataAnnotations;
using LinkyAppBackend.Api.Models.Enums;

namespace LinkyAppBackend.Api.Models.Dtos.GroupUser;

public class CreateGroupUserDto
{
    [Required]
    [MaxLength(255)]
    public string UserName { get; set; } = null!;
    public GroupRole Role { get; set; }
}