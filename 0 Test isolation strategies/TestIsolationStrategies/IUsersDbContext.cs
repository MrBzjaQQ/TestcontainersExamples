using Microsoft.EntityFrameworkCore;

namespace TestIsolationStrategies;

public interface IUsersDbContext
{
    DbSet<User> Users { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}