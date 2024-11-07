using System.Net;
using LinkyAppBackend.Api.Models.Entities.Assoc;
using LinkyAppBackend.Api.Models.Entities.Master;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using File = LinkyAppBackend.Api.Models.Entities.Master.File;

namespace LinkyAppBackend.Api.Models.Entities;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<AppUser, IdentityRole, string>(options)
{
    public DbSet<AppUser> AppUsers { get; set; }
    public DbSet<File> Files { get; set; }
    public DbSet<Link> Links { get; set; }
    public DbSet<LinkGroup> LinkGroups { get; set; }
    public DbSet<LinkGroupUser> LinkGroupUsers { get; set; }
}