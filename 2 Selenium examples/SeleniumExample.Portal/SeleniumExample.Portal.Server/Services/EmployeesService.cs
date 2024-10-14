using MassTransit;
using SeleniumExample.Contract;
using SeleniumExample.Portal.Server.Connectors;
using SeleniumExample.Portal.Server.Dtos;

namespace RabbitMQExamples.API.Services;

public class EmployeesService: IEmployeesService
{
    private readonly IRequestClient<CreateEmployeeRequest> _requestClient;
    private readonly IEmployeesConnector _connector;
    private readonly IBus _bus;

    public EmployeesService(IRequestClient<CreateEmployeeRequest> requestClient, IEmployeesConnector connector)
    {
        _requestClient = requestClient;
        _connector = connector;
    }

    public async Task<ICreateEmployeeResponse> CreateEmployeeAsync(CreateEmployeeRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _requestClient.GetResponse<ICreateEmployeeResponse>(request, cancellationToken);
        return response.Message;
    }

    public async Task<GetEmployeeDto> GetEmployeeAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _connector.GetEmployeeAsync(id, cancellationToken);
    }
}