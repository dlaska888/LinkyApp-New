using System.ComponentModel.DataAnnotations;

namespace LinkyAppBackend.Api.Models.Entities.Master;

public class RefreshToken : AuditableEntity
{
    [Key] public string Token { get; set; } = Guid.NewGuid().ToString();
    public DateTime Expiration { get; set; }
    public string UserId { get; set; } = null!;
    public virtual AppUser User { get; set; } = null!;
}