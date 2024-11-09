using LinkyAppBackend.Api.Extentions;
using LinkyAppBackend.Api.Models.Entities.Assoc;
using LinkyAppBackend.Api.Models.Entities.Interfaces;
using LinkyAppBackend.Api.Models.Entities.Master;
using LinkyAppBackend.Api.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyQueryFilter<IAuditableEntity>(e => e.EntityStatus == EntityStatus.Active);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<IAuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.ModifiedAt = DateTime.UtcNow;
                    break;
                case EntityState.Deleted:
                    HandleSoftDelete(entry);
                    break;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    private static void HandleSoftDelete(EntityEntry<IAuditableEntity> entry)
    {
        entry.State = EntityState.Modified; // Prevent physical deletion
        entry.Entity.EntityStatus = EntityStatus.Deleted;
        entry.Entity.DeletedAt = DateTime.UtcNow;
    }
}