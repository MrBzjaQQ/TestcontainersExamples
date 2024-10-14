using Testcontainers.Kafka;
using Confluent.Kafka;
using FluentAssertions;

namespace KafkaTestContainers;

public class Tests
{
    private const string Topic = "test";
    private const string Message = "This is a test message";
    private KafkaContainer _container;
    
    [SetUp]
    public async Task Setup()
    {
        _container = new KafkaBuilder()
            .Build();
        await _container.StartAsync();
    }

    [TearDown]
    public async Task TearDown()
    {
        await _container.StopAsync();
        await _container.DisposeAsync();
    }

    [Test]
    public async Task ExampleKafkaTest()
    {
        // Arrange
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = _container.Hostname,
        };
        
        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = _container.Hostname,
            GroupId = "foo",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        // Act
        using var producer = new ProducerBuilder<Null, string>(producerConfig).Build();
        await producer.ProduceAsync(Topic, new Message<Null, string> { Value = Message });

        using var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
        var result = consumer.Consume(CancellationToken.None);

        // Assert
        result.Message.Value.Should().Be(Message);
    }
}