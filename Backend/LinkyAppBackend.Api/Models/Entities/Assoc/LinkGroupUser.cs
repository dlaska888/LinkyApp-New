using System.ComponentModel.DataAnnotations;
using LinkyAppBackend.Api.Models.Entities.Master;
using LinkyAppBackend.Api.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Sieve.Attributes;

namespace LinkyAppBackend.Api.Models.Entities.Assoc;

[PrimaryKey(nameof(UserId), nameof(GroupId))]
public class LinkGroupUser : AuditableEntity
{
    [Sieve(CanFilter = true, CanSort = true)]
    public GroupRole Role { get; set; }

    [MaxLength(255)] public string UserId { get; set; } = null!;
    public virtual AppUser User { get; set; } = null!;

    [MaxLength(255)] public string GroupId { get; set; } = null!;
    public virtual LinkGroup Group { get; set; } = null!;
}