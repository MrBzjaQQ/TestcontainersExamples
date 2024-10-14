using MassTransit;
using RabbitMQExamples.API.Services;
using Refit;
using SeleniumExample.Portal.Server.Connectors;
using SeleniumExample.Users.Settings;

namespace RabbitMQExamples.API;

public static class DependencyInjection
{
    public static IServiceCollection AddMassTransit(this IServiceCollection serviceCollection, MassTransitSettings? settings)
    {
        if (settings is null)
        {
            throw new ArgumentException("MassTransit setting are not provided", nameof(settings));
        }
        
        serviceCollection.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(settings.Host, "/", h => {
                    h.Username(settings.UserName);
                    h.Password(settings.Password);
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        return serviceCollection;
    }

    public static IServiceCollection AddConnectors(this IServiceCollection serviceCollection, string? employeesUrl)
    {
        if (string.IsNullOrEmpty(employeesUrl))
        {
            throw new ArgumentException("Employees URL is empty", nameof(employeesUrl));
        }
        
        serviceCollection.AddRefitClient<IEmployeesConnector>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(employeesUrl));
        
        return serviceCollection;
    }

    public static IServiceCollection AddServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IEmployeesService, EmployeesService>();
        return serviceCollection;
    }
}