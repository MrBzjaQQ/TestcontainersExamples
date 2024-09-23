using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace TestIsolationStrategies.NUnit.PerClass;

public class CreateUserTests: TestFixtureBase
{
    [Test]
    public async Task CreateUserAsync_CreateDataProvided_UserIsCreated()
    {
        // Arrange
        var createUserData = new User("Ivanov Ivan Ivanovich");

        // Act
        await ProceedWithContextAsync(async context =>
        {
            var service = new UsersService(context);
            await service.CreateUserAsync(createUserData);
        });

        // Assert
        await ProceedWithContextAsync(async context =>
        {
            var user = await context.Users.SingleAsync(user => user.Id == createUserData.Id);
            user.Name.Should().Be(createUserData.Name);
        });
    }
}