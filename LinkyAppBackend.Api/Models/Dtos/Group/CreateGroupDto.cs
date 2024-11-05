using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace LinkyAppBackend.Api.Models.Dtos.Group;

public class CreateGroupDto
{
    [MaxLength(255)] public string Name { get; set; } = null!;
    [MaxLength(255)] public string? Description { get; set; }
}