using Microsoft.EntityFrameworkCore;
using SeleniumExample.Users.Entities;

namespace SeleniumExample.Users.Context;

public class EmployeesDbContext(DbContextOptions<EmployeesDbContext> options) : DbContext(options), IEmployeesDbContext
{
    public DbSet<Employee> Employees { get; set; }
}