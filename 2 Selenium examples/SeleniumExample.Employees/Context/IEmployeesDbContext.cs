using Microsoft.EntityFrameworkCore;
using SeleniumExample.Users.Entities;

namespace SeleniumExample.Users.Context;

public interface IEmployeesDbContext
{
    public DbSet<Employee> Employees { get; }
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}