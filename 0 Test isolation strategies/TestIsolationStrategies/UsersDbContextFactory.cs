using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TestIsolationStrategies;

public class UsersDbContextFactory: IDesignTimeDbContextFactory<UsersDbContext>
{
    public UsersDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<UsersDbContext>()
            .UseNpgsql()
            .Options;

        return new UsersDbContext(options);
    }
}