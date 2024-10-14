using Microsoft.AspNetCore.Mvc;
using RabbitMQExamples.API.Services;
using SeleniumExample.Contract;
using SeleniumExample.Portal.Server.Dtos;

namespace SeleniumExample.Portal.Server.Controllers;

[ApiController]
[Route("employees")]
public class EmployeesController: ControllerBase
{
    private readonly IEmployeesService _service;

    public EmployeesController(IEmployeesService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ICreateEmployeeResponse> CreateEmployeeAsync(CreateEmployeeRequest request)
    {
        return await _service.CreateEmployeeAsync(request, HttpContext.RequestAborted);
    }

    [HttpGet("{id}")]
    public async Task<GetEmployeeDto> GetEmployeeAsync(Guid id)
    {
        return await _service.GetEmployeeAsync(id, HttpContext.RequestAborted);
    }
}