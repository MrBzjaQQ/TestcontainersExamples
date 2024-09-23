using Microsoft.EntityFrameworkCore;
using Xunit.Extensions.AssemblyFixture;

// [assembly: AssemblyFixture(typeof(TargetDbFixture))] - Will be released with xUnit 3.0
// according to https://xunit.net/docs/shared-context#assembly-fixture

[assembly: TestFramework(AssemblyFixtureFramework.TypeName, AssemblyFixtureFramework.AssemblyName)]
namespace TestIsolationStrategies.xUnit.PerAssembly.Infrastructure;

public abstract class TargetDbTestsBase: IAsyncLifetime, IAssemblyFixture<TargetDbFixture>
{
    protected readonly string _connectionString;

    protected TargetDbTestsBase(TargetDbFixture fixture)
    {
        _connectionString = fixture.ConnectionString;
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
        using var context = TargetEnvironment.CreateDbContext(_connectionString);
        await action.Invoke(context);
    }

    protected void ProceedWithContext(Action<UsersDbContext> action)
    {
        using var context = TargetEnvironment.CreateDbContext(_connectionString);
        action.Invoke(context);
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        await ProceedWithContextAsync(ClearDatabase);
    }

    private async Task ClearDatabase(UsersDbContext context)
    {
        var tableNames = context.Model.GetEntityTypes()
            .Select(et => et.GetTableName())
            .Distinct();

        foreach (var tableName in tableNames)
        {
#pragma warning disable EF1002 // SQL Injection is not possible due to project of application tests
            await context.Database.ExecuteSqlRawAsync($"DELETE FROM \"{tableName}\";");
#pragma warning restore EF1002 // SQL Injection is not possible due to project of application tests
        }
    }
}