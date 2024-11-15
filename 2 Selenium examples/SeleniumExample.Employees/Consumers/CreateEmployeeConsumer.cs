using MassTransit;
using SeleniumExample.Contract;
using SeleniumExample.Employees.Context;
using SeleniumExample.Employees.Dtos;
using SeleniumExample.Employees.Entities;

namespace SeleniumExample.Employees.Consumers;

public class CreateEmployeeConsumer: IConsumer<ICreateEmployeeRequest>
{
    private readonly IEmployeesDbContext _dbContext;

    public CreateEmployeeConsumer(IEmployeesDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task Consume(ConsumeContext<ICreateEmployeeRequest> context)
    {
        var employee = new Employee(
            context.Message.Phone,
            context.Message.UserName,
            context.Message.Email,
            context.Message.Position);

        await _dbContext.Employees.AddAsync(employee);
        await _dbContext.SaveChangesAsync(CancellationToken.None);

        await context.RespondAsync<ICreateEmployeeResponse>(new CreateEmployeeResponse
        {
            Id = employee.Id
        });
    }
}