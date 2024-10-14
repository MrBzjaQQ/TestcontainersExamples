using MassTransit;
using Microsoft.EntityFrameworkCore;
using SeleniumExample.Users.Consumers;
using SeleniumExample.Users.Context;
using SeleniumExample.Users.Settings;

namespace SeleniumExample.Users;

public static class DependencyInjection
{
    public static IServiceCollection AddMassTransit(
        this IServiceCollection serviceCollection,
        MassTransitSettings? massTransitSettings)
    {
        if (massTransitSettings is null)
        {
            throw new ArgumentException("MassTransit settings is not initialized", nameof(massTransitSettings));
        }
        
        serviceCollection.AddMassTransit(x =>
        {
            x.AddConsumer<CreateEmployeeConsumer>();
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(massTransitSettings.Host, "/", h => {
                    h.Username(massTransitSettings.UserName);
                    h.Password(massTransitSettings.Password);
                });

                cfg.ConfigureEndpoints(context);
            });
        });
        
        return serviceCollection;
    }

    public static IServiceCollection AddDatabase(
        this IServiceCollection serviceCollection,
        string? connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentException("ConnectionString is not initialized", nameof(connectionString));
        }
        serviceCollection.AddDbContext<EmployeesDbContext>(x => x.UseNpgsql(connectionString));
        serviceCollection.AddScoped<IEmployeesDbContext>(x => x.GetRequiredService<EmployeesDbContext>());
        serviceCollection.AddScoped<EmployeesDbMigrator>();
        
        return serviceCollection;
    }
}