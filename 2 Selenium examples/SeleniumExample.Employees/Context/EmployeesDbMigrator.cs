using Microsoft.EntityFrameworkCore;

namespace SeleniumExample.Employees.Context;

public class EmployeesDbMigrator
{
    private readonly EmployeesDbContext _dbContext;

    public EmployeesDbMigrator(EmployeesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task MigrateAsync()
    {
        await _dbContext.Database.MigrateAsync();
    }
}