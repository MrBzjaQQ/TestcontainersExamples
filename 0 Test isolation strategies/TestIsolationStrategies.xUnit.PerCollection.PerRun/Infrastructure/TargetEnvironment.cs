using Microsoft.EntityFrameworkCore;

namespace TestIsolationStrategies.xUnit.PerCollection.Infrastructure;

internal static class TargetEnvironment
{
    internal static UsersDbContext CreateDbContext(string connectionString)
    {
        var options = new DbContextOptionsBuilder<UsersDbContext>()
            .UseNpgsql(connectionString)
            .Options;
        return new UsersDbContext(options);
    }
}