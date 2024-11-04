using System.ComponentModel.DataAnnotations;
using LinkyAppBackend.Api.Models.Entities.Interfaces;
using Sieve.Attributes;

namespace LinkyAppBackend.Api.Models.Entities;

public class Entity : IEntity
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    [Sieve(CanFilter = true, CanSort = true)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}