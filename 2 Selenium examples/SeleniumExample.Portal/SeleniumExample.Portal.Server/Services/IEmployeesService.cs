using SeleniumExample.Contract;
using SeleniumExample.Portal.Server.Dtos;

namespace RabbitMQExamples.API.Services;

public interface IEmployeesService
{
    Task<ICreateEmployeeResponse> CreateEmployeeAsync(
        CreateEmployeeRequest request,
        CancellationToken cancellationToken = default);

    Task<GetEmployeeDto> GetEmployeeAsync(Guid id, CancellationToken cancellationToken = default);
}
