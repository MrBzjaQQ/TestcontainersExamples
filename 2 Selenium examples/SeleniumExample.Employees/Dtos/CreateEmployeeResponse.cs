using SeleniumExample.Contract;

namespace SeleniumExample.Users.Dtos;

public class CreateEmployeeResponse: ICreateEmployeeResponse
{
    public Guid Id { get; init; }
}