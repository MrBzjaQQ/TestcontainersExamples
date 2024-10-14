namespace SeleniumExample.Users.Settings;

public sealed record DatabaseSettings
{
    public string ConnectionString { get; init; } = string.Empty;
}