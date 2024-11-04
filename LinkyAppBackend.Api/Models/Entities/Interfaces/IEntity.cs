using LinkyAppBackend.Api.Models.Enums;

namespace LinkyAppBackend.Api.Models.Entities.Interfaces;

public interface IAuditableEntity
{
    EntityStatus EntityStatus { get; set; }
    DateTime CreatedAt { get; set; }
    DateTime ModifiedAt { get; set; }
    DateTime? DeletedAt { get; set; }
}