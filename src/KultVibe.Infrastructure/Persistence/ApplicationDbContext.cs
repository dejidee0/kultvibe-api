using KultVibe.Domain.Common;
using KultVibe.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KultVibe.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<WaitlistEntry> WaitlistEntries => Set<WaitlistEntry>();

    // Phase 2+: Users, RefreshTokens, OtpCodes, ...
    // Phase 3+: Tournaments, Matches, Transactions, ...
    // Each phase adds its DbSets here plus a matching IEntityTypeConfiguration in Persistence/Configurations.

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
        }
    }
}
