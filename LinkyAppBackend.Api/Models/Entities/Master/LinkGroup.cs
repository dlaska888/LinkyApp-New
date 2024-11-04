using System.ComponentModel.DataAnnotations;
using LinkyAppBackend.Api.Models.Entities.Assoc;
using Sieve.Attributes;

namespace LinkyAppBackend.Api.Models.Entities.Master;

public class LinkGroup : Entity
{
    [Sieve(CanFilter = true, CanSort = true)]
    [MaxLength(255)]
    public string Name { get; set; } = null!;

    [Sieve(CanFilter = true)]
    [MaxLength(255)]
    public string? Description { get; set; }

    public virtual ICollection<Link> Links { get; set; } = [];
    public virtual ICollection<LinkGroupUser> Users { get; set; } = [];
}