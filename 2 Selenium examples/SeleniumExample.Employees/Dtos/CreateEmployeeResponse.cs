using SeleniumExample.Contract;

namespace SeleniumExample.Employees.Dtos;

public class CreateEmployeeResponse: ICreateEmployeeResponse
{
    public Guid Id { get; init; }
}