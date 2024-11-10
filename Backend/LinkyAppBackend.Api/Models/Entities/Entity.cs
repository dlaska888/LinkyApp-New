using System.ComponentModel.DataAnnotations;
using LinkyAppBackend.Api.Models.Entities.Interfaces;
using LinkyAppBackend.Api.Models.Enums;
using Sieve.Attributes;

namespace LinkyAppBackend.Api.Models.Entities;

public class Entity : IKeyedEntity, IAuditableEntity
{
    [Key] public string Id { get; set; } = Guid.NewGuid().ToString();

    [Sieve(CanFilter = true, CanSort = true)]
    public EntityStatus EntityStatus { get; set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Sieve(CanFilter = true, CanSort = true)]
    public DateTime ModifiedAt { get; set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public DateTime? DeletedAt { get; set; }
}