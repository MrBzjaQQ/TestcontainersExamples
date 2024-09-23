using Microsoft.EntityFrameworkCore;

namespace TestIsolationStrategies;
public class UsersService
{
    private readonly IUsersDbContext _context;

    public UsersService(IUsersDbContext context)
    {
        _context = context;
    }

    public async Task CreateUserAsync(User user, CancellationToken cancellationToken = default)
    {
        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteUserAsync(long id, CancellationToken cancellationToken = default)
    {
        await _context.Users.Where(user => user.Id == id).ExecuteDeleteAsync(cancellationToken);
    }

    public async Task UpdateUserAsync(long id, string name, CancellationToken cancellationToken = default)
    {
        var user = await _context.Users.SingleAsync(user => user.Id == id, cancellationToken);
        user.SetName(name);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IList<User>> GetUsersAsync(int take, int skip, CancellationToken cancellationToken = default)
    {
        return await _context.Users.Skip(skip).Take(take).ToListAsync(cancellationToken);
    }

    public async Task<User> GetUserAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.Users.SingleAsync(user => user.Id == id, cancellationToken);
    }
}
