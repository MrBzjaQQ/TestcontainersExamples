using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Testcontainers.RabbitMq;

namespace RabbitMQExamples;
public abstract class TestsBase
{
    private RabbitMqContainer _container;

    [SetUp]
    public async Task Setup()
    {
        _container = new RabbitMqBuilder().Build();
        await _container.StartAsync();
    }

    [TearDown]
    public async Task TearDown()
    {
        await _container.DisposeAsync();
    }
}
