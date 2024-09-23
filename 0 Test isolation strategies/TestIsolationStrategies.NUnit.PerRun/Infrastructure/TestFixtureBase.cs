using Microsoft.EntityFrameworkCore;

namespace TestIsolationStrategies.NUnit.PerRun.Infrastructure;
public abstract class TestFixtureBase
{
    [SetUp]
    public virtual async Task SetUp()
    {
        // TODO
    }

    [TearDown]
    public async Task TearDown()
    {
        await ProceedWithContextAsync(ClearDatabase);
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
        using var context = TargetEnvironment.CreateDbContext(TestModule.Current.GetDbConnectionString());
        await action.Invoke(context);
    }

    protected void ProceedWithContext(Action<UsersDbContext> action)
    {
        using var context = TargetEnvironment.CreateDbContext(TestModule.Current.GetDbConnectionString());
        action.Invoke(context);
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
