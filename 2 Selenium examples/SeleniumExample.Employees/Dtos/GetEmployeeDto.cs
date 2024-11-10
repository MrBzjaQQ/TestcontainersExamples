namespace SeleniumExample.Employees.Dtos;

public sealed record GetEmployeeDto(Guid Id, string Phone, string UserName, string Email, string Position);