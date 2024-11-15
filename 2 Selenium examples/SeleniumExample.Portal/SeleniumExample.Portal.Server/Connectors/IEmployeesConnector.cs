using Refit;
using SeleniumExample.Portal.Server.Dtos;

namespace SeleniumExample.Portal.Server.Connectors;

public interface IEmployeesConnector
{
    [Get("/employees/{id}")]
    public Task<GetEmployeeDto> GetEmployeeAsync(Guid id, CancellationToken cancellationToken);
}