namespace SeleniumExample.Employees.Settings;

public sealed record AppSettings
{
    public MassTransitSettings MassTransit { get; init; } = new();
    public string EmployeesUrl { get; init; } = string.Empty;
}