using Refit;
using SeleniumExample.Portal.Server.Dtos;

namespace RabbitMQExamples;

public interface IPortalService
{
    [Post("/api/employees")]
    public Task<CreateEmployeeResponse> CreateEmployeeAsync(CreateEmployeeRequest request, CancellationToken cancellationToken = default);

    [Get("/api/employees/{id}")]
    public Task<GetEmployeeDto> GetEmployeeAsync(Guid id, CancellationToken cancellationToken = default);
}