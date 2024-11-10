using Refit;
using SeleniumExample.Portal.Server.Dtos;

namespace RabbitMQExamples;

public interface IPortalService
{
    [Post("/employees")]
    public Task<CreateEmployeeResponse> CreateEmployeeAsync(CreateEmployeeRequest request, CancellationToken cancellationToken = default);

    [Get("/employees/{id}")]
    public Task<GetEmployeeDto> GetEmployeeAsync(Guid id, CancellationToken cancellationToken = default);
}