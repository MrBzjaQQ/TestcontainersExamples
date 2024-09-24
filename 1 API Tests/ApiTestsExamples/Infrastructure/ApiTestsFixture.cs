using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;

namespace ApiTestsExamples.Infrastructure;

public class ApiTestsFixture: IAsyncLifetime
{
    private IContainer _container;

    public async Task InitializeAsync()
    {
        _container = CreateContainer();
        await _container.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _container.StopAsync();
        await _container.DisposeAsync();
    }

    public Uri GetUri()
    {
        return new UriBuilder(
                Uri.UriSchemeHttp, 
                _container.Hostname,
                _container.GetMappedPublicPort(80), 
                "/").Uri;
    }

    private static IContainer CreateContainer()
    {
        return new ContainerBuilder()
            .WithImage("nginx")
            .WithPortBinding(80, true)
            .Build();
    }
}