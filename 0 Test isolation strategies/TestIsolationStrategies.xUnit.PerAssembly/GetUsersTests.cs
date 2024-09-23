﻿using FluentAssertions;
using TestIsolationStrategies.xUnit.PerAssembly.Infrastructure;

namespace TestIsolationStrategies.xUnit.PerAssembly;

public class GetUsersTests: TargetDbTestsBase
{
    public GetUsersTests(TargetDbFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task GetUserAsync()
    {
        // Arrange
        var user = new User("Ivanov Ivan Ivanovich");
        await ProceedWithContextAsync(async context =>
        {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
        });

        // Act
        var userFromGetMethod = await ProceedWithContextAsync(async context =>
        {
            var service = new UsersService(context);
            return await service.GetUserAsync(user.Id);
        });

        // Assert
        userFromGetMethod.Should().BeEquivalentTo(user);
    }

    [Fact]
    public async Task GetUsersAsync()
    {
        // Arrange
        var user1 = new User("Ivanov Ivan Ivanovich");
        var user2 = new User("Sergeev Sergey Sergeevich");
        await ProceedWithContextAsync(async context =>
        {
            await context.Users.AddAsync(user1);
            await context.Users.AddAsync(user2);
            await context.SaveChangesAsync();
        });

        // Act
        var users = await ProceedWithContextAsync(async context =>
        {
            var service = new UsersService(context);
            return await service.GetUsersAsync(5, 0);
        });

        // Assert
        users.Should().BeEquivalentTo(new[]
        {
            user1,
            user2
        });
    }
}
