using LinkyAppBackend.Api.Models.Entities.Interfaces;
using LinkyAppBackend.Api.Models.Enums;

namespace LinkyAppBackend.Api.Models.Entities;

public class AssocEntity : IAuditableEntity
{
    public EntityStatus EntityStatus { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}