using LinkyAppBackend.Api.Models.Dtos.Interfaces;

namespace LinkyAppBackend.Api.Models.Dtos;

public class GetDto : IGetDto
{
    public string Id { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}