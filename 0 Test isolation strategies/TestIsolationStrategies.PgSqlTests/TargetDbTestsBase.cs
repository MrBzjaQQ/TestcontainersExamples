using Microsoft.EntityFrameworkCore;

namespace TestIsolationStrategies.xUnit.PerTest;

public abstract class TargetDbTestsBase: IAsyncLifetime
{
    private const string ContainerStartedLog = "PostgreSQL init process complete; ready for start up.";
    private string ConnectionString = $"User ID=postgres;Password=postgres;Host=127.0.0.1;Port=5432;Database=tests-{Guid.NewGuid()};";

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
        await ProceedWithContextAsync(async context =>
        {
            await context.Database.MigrateAsync();
        });
    }

    public async Task DisposeAsync()
    {
        await ProceedWithContextAsync(async context =>
        {
            await context.Database.EnsureDeletedAsync();
        });
    }

    private static UsersDbContext CreateDbContext(string connectionString)
    {
        var options = new DbContextOptionsBuilder<UsersDbContext>()
            .UseNpgsql(connectionString)
            .Options;
        return new UsersDbContext(options);
    }
}