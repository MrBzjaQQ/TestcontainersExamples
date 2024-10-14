namespace SeleniumExample.Users.Settings;

public sealed record AppSettings
{
    public MassTransitSettings MassTransit { get; init; } = new();
    public DatabaseSettings Database { get; init; } = new();
}