using System.ComponentModel.DataAnnotations;
using ApiASPNET.Api.Models.Entities.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace ApiASPNET.Api.Models.Entities.Master;

public class AppUser : IdentityUser, IEntity
{
    [MaxLength(255)] public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExp { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}