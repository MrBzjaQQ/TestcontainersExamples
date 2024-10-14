using SeleniumExample.Contract;

namespace SeleniumExample.Portal.Server.Dtos;

public sealed record CreateEmployeeRequest(string Phone, string UserName, string Email, string Position): ICreateEmployeeRequest;