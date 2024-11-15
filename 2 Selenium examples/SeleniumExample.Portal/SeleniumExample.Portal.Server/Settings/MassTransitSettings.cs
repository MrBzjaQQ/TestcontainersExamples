namespace SeleniumExample.Employees.Settings;

public sealed record MassTransitSettings
{
    public string Host { get; init; } = string.Empty;
    public string UserName { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}