using System.ComponentModel.DataAnnotations;

namespace LinkyAppBackend.Api.Models.Dtos.Link;

public class CreateLinkDto
{
    [MaxLength(50)] public string Title { get; set; } = null!;
    [MaxLength(2048)] public string Url { get; set; } = null!;
}