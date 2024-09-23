using DotNet.Testcontainers.Builders;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace TestIsolationStrategies.NUnit.PerClass;

[TestFixture]
public abstract class TestFixtureBase
{
    private PostgreSqlContainer? _dbContainer;
    private const string DbContainerIsNotCreated = "PostgreSQL container is not created";
    private const string ContainerStartedLog = "PostgreSQL init process complete; ready for start up.";

    [OneTimeSetUp]
    public virtual async Task OneTimeSetUp()
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
        await ProceedWithContextAsync(ClearDatabase);
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
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

    private async Task ClearDatabase(UsersDbContext db)
    {
        var tableNames = db.Model.GetEntityTypes()
            .Select(et => et.GetTableName())
            .Distinct();

        foreach (var tableName in tableNames)
        {
#pragma warning disable EF1002 // SQL Injection is not possible due to project of application tests
            await db.Database.ExecuteSqlRawAsync($"DELETE FROM \"{tableName}\";");
#pragma warning restore EF1002 // SQL Injection is not possible due to project of application tests
        }
    }
}
