using System.ComponentModel.DataAnnotations;
using LinkyAppBackend.Api.Models.Entities.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace LinkyAppBackend.Api.Models.Entities.Master;

public class AppUser : IdentityUser, IEntity
{
    [MaxLength(255)] public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExp { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}