using LinkyAppBackend.Api.Models.Dtos.Interfaces;
using LinkyAppBackend.Api.Models.Enums;

namespace LinkyAppBackend.Api.Models.Dtos;

public class GetDto : IGetDto
{
    public string Id { get; set; } = null!;
    public EntityStatus EntityStatus { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}