using SeleniumExample.Contract;

namespace SeleniumExample.Portal.Server.Dtos;

public class CreateEmployeeResponse: ICreateEmployeeResponse
{
    public Guid Id { get; init; }
}