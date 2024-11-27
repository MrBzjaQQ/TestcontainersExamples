using DotNet.Testcontainers.Builders;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace TestIsolationStrategies.xUnit.PerTest;

public abstract class TargetDbTestsBase: IAsyncLifetime
{
    private const string ContainerStartedLog = "PostgreSQL init process complete; ready for start up.";
    private PostgreSqlContainer _dbContainer { get; set; }
    private string ConnectionString => _dbContainer.GetConnectionString();

    protected async Task<T> ProceedWithContextAsync<T>(Func<UsersDbContext, Task<T>> action)
    {
        T result = default;
        await ProceedWithContextAsync(async context =>
        {
            result = await action(context);
        });
        return result;
    }

    protected async Task ProceedWithContextAsync(Func<UsersDbContext, Task> action)
    {
        using var context = CreateDbContext(ConnectionString);
        await action.Invoke(context);
    }

    protected void ProceedWithContext(Action<UsersDbContext> action)
    {
        using var context = CreateDbContext(ConnectionString);
        action.Invoke(context);
    }

    public async Task InitializeAsync()
    {
        _dbContainer = CreateDbContainer();
        await _dbContainer.StartAsync();
        await ProceedWithContextAsync(async context => await context.Database.MigrateAsync());
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

    private static UsersDbContext CreateDbContext(string connectionString)
    {
        var options = new DbContextOptionsBuilder<UsersDbContext>()
            .UseNpgsql(connectionString)
            .Options;
        return new UsersDbContext(options);
    }
}