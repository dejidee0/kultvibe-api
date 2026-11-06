using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace KultVibe.Infrastructure.Persistence;

/// <summary>
/// Creates the model for migration generation without running API startup or loading credentials.
/// </summary>
public sealed class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        // Model generation needs the provider, but no database connection.
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer()
            .Options;

        return new ApplicationDbContext(options);
    }
}
