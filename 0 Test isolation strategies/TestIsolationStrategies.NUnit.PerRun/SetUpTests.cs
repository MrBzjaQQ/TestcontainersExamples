using DotNet.Testcontainers.Builders;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;
using TestIsolationStrategies.NUnit.PerRun.Infrastructure;

namespace TestIsolationStrategies.NUnit.PerRun;

[SetUpFixture]
public class SetUpTests
{
    private const string ContainerStartedLog = "PostgreSQL init process complete; ready for start up.";

    [OneTimeSetUp]
    public async Task RunBeforeTests()
    {
        var container = CreateDbContainer();
        TestModule.Current.SetDbContainer(container);
        await TestModule.Current.StartDbContainerAsync();
        var dbContext = TargetEnvironment.CreateDbContext(TestModule.Current.GetDbConnectionString());
        await dbContext.Database.MigrateAsync();
    }

    [OneTimeTearDown]
    public async Task RunAfterTests()
    {
        await TestModule.Current.StopDbContainerAsync();
        await TestModule.Current.DisposeDbContainerAsync();
    }

    private static PostgreSqlContainer CreateDbContainer()
    {
        var postgreSqlBuilder = new PostgreSqlBuilder()
            .WithImage(PostgreSqlBuilder.PostgreSqlImage)
            .WithPortBinding(PostgreSqlBuilder.PostgreSqlPort, assignRandomHostPort: true)
            .WithWaitStrategy(Wait.ForUnixContainer()
                .UntilMessageIsLogged(ContainerStartedLog)
                .UntilPortIsAvailable(5432));

        return postgreSqlBuilder.Build();
    }
}
