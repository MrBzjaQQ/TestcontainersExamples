using DotNet.Testcontainers.Builders;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace TestIsolationStrategies.xUnit.PerClass.Infrastructure;

public class TargetDbFixture : IAsyncLifetime
{
    private const string ContainerStartedLog = "PostgreSQL init process complete; ready for start up.";
    private readonly PostgreSqlContainer _dbContainer = CreateDbContainer();
    public string ConnectionString => _dbContainer.GetConnectionString();

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        var context = TargetEnvironment.CreateDbContext(ConnectionString);
        await context.Database.MigrateAsync();
    }

    public async Task DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
    }

    private static PostgreSqlContainer CreateDbContainer()
        => new PostgreSqlBuilder()
            .WithDatabase($"tests-{Guid.NewGuid()}")
            .WithPortBinding(PostgreSqlBuilder.PostgreSqlPort, assignRandomHostPort: true)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilMessageIsLogged(ContainerStartedLog))
            .Build();
}