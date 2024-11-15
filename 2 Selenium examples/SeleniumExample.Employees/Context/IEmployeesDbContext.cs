using Microsoft.EntityFrameworkCore;
using SeleniumExample.Employees.Entities;

namespace SeleniumExample.Employees.Context;

public interface IEmployeesDbContext
{
    public DbSet<Employee> Employees { get; }
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}