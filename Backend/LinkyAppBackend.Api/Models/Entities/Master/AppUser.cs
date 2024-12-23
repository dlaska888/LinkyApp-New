using System.ComponentModel.DataAnnotations;
using LinkyAppBackend.Api.Models.Entities.Assoc;
using LinkyAppBackend.Api.Models.Entities.Interfaces;
using LinkyAppBackend.Api.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LinkyAppBackend.Api.Models.Entities.Master;

public class AppUser : IdentityUser, IKeyedEntity, IAuditableEntity
{
    [MaxLength(255)] public string? ProfilePhotoId { get; set; }
    [DeleteBehavior(DeleteBehavior.SetNull)]
    public File? ProfilePhoto { get; set; }

    public virtual ICollection<LinkGroupUser> Groups { get; set; } = [];
    public virtual ICollection<RefreshToken> RefreshTokens { get; } = [];
    public EntityStatus EntityStatus { get; set; } = EntityStatus.Active;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeletedAt { get; set; }
}