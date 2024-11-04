using System.ComponentModel.DataAnnotations;
using Sieve.Attributes;

namespace LinkyAppBackend.Api.Models.Entities.Master;

public class Link : Entity
{
    [Sieve(CanFilter = true, CanSort = true)]
    [MaxLength(50)]
    public string Title { get; set; } = null!;
    
    [Sieve(CanFilter = true, CanSort = true)]
    [MaxLength(2048)]
    public string Url { get; set; } = null!;

    public string CreatorId { get; set; } = null!;
    public virtual AppUser Creator { get; set; } = null!;
}