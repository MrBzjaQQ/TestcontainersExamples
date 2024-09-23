using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace TestIsolationStrategies.NUnit.PerTest;

public class UpdateUserTests: TestFixtureBase
{
    [Test]
    public async Task UpdateUserAsync_UpdateDataProvided_UserIsUpdated()
    {
        // Arrange
        const string newName = "Sergeev Sergey Sergeevich";
        var createdUser = new User("Ivanov Ivan Ivanovich");
        await ProceedWithContextAsync(async context =>
        {
            var service = new UsersService(context);
            await service.CreateUserAsync(createdUser);
        });

        // Act
        await ProceedWithContextAsync(async context =>
        {
            var service = new UsersService(context);
            await service.UpdateUserAsync(createdUser.Id, newName);
        });

        // Assert
        await ProceedWithContextAsync(async context =>
        {
            var user = await context.Users.SingleAsync(user => user.Id == createdUser.Id);
            user.Name.Should().Be(newName);
        });
    }
}