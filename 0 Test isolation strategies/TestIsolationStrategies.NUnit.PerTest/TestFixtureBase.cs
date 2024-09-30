using DotNet.Testcontainers.Builders;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace TestIsolationStrategies.NUnit.PerTest;
public abstract class TestFixtureBase
{
    private PostgreSqlContainer? _dbContainer;
    private const string DbContainerIsNotCreated = "PostgreSQL container is not created";
    private const string ContainerStartedLog = "PostgreSQL init process complete; ready for start up.";

    [SetUp]
    public virtual async Task SetUp()
    {
        CreateDbContainer();
        await StartDbContainerAsync();
        await ProceedWithContextAsync(async context =>
        {
            await context.Database.MigrateAsync();
        });
    }

    [TearDown]
    public async Task TearDown()
    {
        await StopDbContainerAsync();
        await DisposeDbContainerAsync();
    }

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
        using var context = CreateDbContext(GetDbConnectionString());
        await action.Invoke(context);
    }

    protected void ProceedWithContext(Action<UsersDbContext> action)
    {
        using var context = CreateDbContext(GetDbConnectionString());
        action.Invoke(context);
    }

    private void CreateDbContainer()
    {
        var postgreSqlBuilder = new PostgreSqlBuilder()
            .WithImage(PostgreSqlBuilder.PostgreSqlImage)
            .WithPortBinding(PostgreSqlBuilder.PostgreSqlPort, assignRandomHostPort: true)
            .WithWaitStrategy(Wait.ForUnixContainer()
                .UntilMessageIsLogged(ContainerStartedLog)
                .UntilPortIsAvailable(5432));

        _dbContainer = postgreSqlBuilder.Build();
    }

    private async Task StartDbContainerAsync()
    {
        if (_dbContainer == null)
        {
            throw new InvalidOperationException(DbContainerIsNotCreated);
        }

        await _dbContainer.StartAsync();
    }

    private async Task StopDbContainerAsync()
    {
        if (_dbContainer == null)
        {
            throw new InvalidOperationException(DbContainerIsNotCreated);
        }

        await _dbContainer.StopAsync();
    }

    private string GetDbConnectionString()
    {
        if (_dbContainer == null)
        {
            throw new InvalidOperationException(DbContainerIsNotCreated);
        }

        return _dbContainer.GetConnectionString();
    }

    private async Task DisposeDbContainerAsync()
    {
        if (_dbContainer == null)
        {
            throw new InvalidOperationException(DbContainerIsNotCreated);
        }

        await _dbContainer.DisposeAsync();
    }

    private static UsersDbContext CreateDbContext(string connectionString)
    {
        var options = new DbContextOptionsBuilder<UsersDbContext>()
            .UseNpgsql(connectionString)
            .Options;

        return new UsersDbContext(options);
    }
}
