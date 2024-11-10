using Microsoft.EntityFrameworkCore;
using SeleniumExample.Employees.Entities;

namespace SeleniumExample.Employees.Context;

public class EmployeesDbContext(DbContextOptions<EmployeesDbContext> options) : DbContext(options), IEmployeesDbContext
{
    public DbSet<Employee> Employees { get; set; }
}