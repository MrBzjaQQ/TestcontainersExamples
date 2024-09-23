using Microsoft.EntityFrameworkCore;

namespace TestIsolationStrategies;
public sealed class UsersDbContext: DbContext, IUsersDbContext
{
    public UsersDbContext(DbContextOptions<UsersDbContext> options)
        : base(options)
    {
        
    }

    public DbSet<User> Users { get; set; }
}
