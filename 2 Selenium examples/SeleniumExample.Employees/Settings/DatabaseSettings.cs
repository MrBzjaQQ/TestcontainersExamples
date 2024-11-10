namespace SeleniumExample.Employees.Settings;

public sealed record DatabaseSettings
{
    public string ConnectionString { get; init; } = string.Empty;
}