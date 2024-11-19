using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Images;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using SeleniumExample.TestsWithWebDriver;
using System.Text.RegularExpressions;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;
using Testcontainers.WebDriver;

namespace RabbitMQExamples;
public abstract class TestsBase
{
    public TestPageObject PageObject { get; set; }
    private const string PortalDockerfileDir = "";
    private const string EmployeesDockerfileDir = "";
    private const string PortalImageName = "selenium-example-portal-image:1.0";
    private const string EmployeesImageName = "selenium-example-employees-image:1.0";
    private const string PortalContainerName = "selenium-example-portal-container";
    private const string EmployeesContainerName = "selenium-example-employees-container";
    private const string PortalDockerfileName = "Dockerfile.portal";
    private const string EmployeesDockerfileName = "Dockerfile.employees";
    private const string ContainerStartedLog = "PostgreSQL init process complete; ready for start up.";

    private RabbitMqContainer _rabbitMqContainer;
    private PostgreSqlContainer _postgreSqlContainer;
    private IFutureDockerImage _portalImage;
    private IFutureDockerImage _employeesImage;

    private IContainer _portalContainer;
    private IContainer _employeesContainer;
    private WebDriverContainer _webDriverContainer;

    private IWebDriver _driver { get; set; }

    [SetUp]
    public async Task Setup()
    {
        _rabbitMqContainer = BuildRabbitMQContainer();
        _postgreSqlContainer = BuildPostgreSqlContainer();
        var createPortalImageTask = BuildAndCreateImage(PortalDockerfileDir, PortalDockerfileName, PortalImageName);
        var createEmployeesImageTask = BuildAndCreateImage(EmployeesDockerfileDir, EmployeesDockerfileName, EmployeesImageName);
        await Task.WhenAll(createPortalImageTask, createEmployeesImageTask);

        _portalImage = createPortalImageTask.Result;
        _employeesImage = createEmployeesImageTask.Result;
        var startRabbitTask = _rabbitMqContainer.StartAsync();
        var startPostgresTask = _postgreSqlContainer.StartAsync();
        await Task.WhenAll(startRabbitTask, startPostgresTask);

        _employeesContainer = BuildEmployeesContainer();
        _portalContainer = BuildPortalContainer();
        await _employeesContainer.StartAsync();
        await _portalContainer.StartAsync();

        _webDriverContainer = BuildWebDriverContainer();
        await _webDriverContainer.StartAsync();
        _driver = new RemoteWebDriver(new Uri(_webDriverContainer.GetConnectionString()), new ChromeOptions());
        PageObject = new TestPageObject(_driver);
        
    }

    [TearDown]
    public async Task TearDown()
    {
        _driver.Dispose();
        await _webDriverContainer.StopAsync();
        await _webDriverContainer.ExportVideoAsync(@"video.mp4");

        await _rabbitMqContainer.DisposeAsync();
        await _portalContainer.DisposeAsync();
        await _employeesContainer.DisposeAsync();
        await _portalImage.DisposeAsync();
        await _employeesImage.DisposeAsync();
        await _postgreSqlContainer.DisposeAsync();
        await _webDriverContainer.DisposeAsync();
    }

    private RabbitMqContainer BuildRabbitMQContainer()
    {
        return new RabbitMqBuilder()
            .WithUsername("guest")
            .WithPassword("guest")
            .WithPortBinding(15672, 15672)
            .WithPortBinding(5672, 5672)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5672))
            .Build();
    }

    private PostgreSqlContainer BuildPostgreSqlContainer()
    {
        return new PostgreSqlBuilder()
            .WithPortBinding(5432, 5432)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilMessageIsLogged(ContainerStartedLog))
            .Build();
    }
    
    private async Task<IFutureDockerImage> BuildAndCreateImage(string dockerfileDir, string dockerFileName, string imageName)
    {
        var image = new ImageFromDockerfileBuilder()
            .WithDockerfileDirectory(CommonDirectoryPath.GetSolutionDirectory(), dockerfileDir)
            .WithDockerfile(dockerFileName)
            .WithName(imageName)
            .Build();

        await image.CreateAsync();
        return image;
    }

    private IContainer BuildPortalContainer()
    {
        return new ContainerBuilder()
            .WithImage(_portalImage)
            .WithName(PortalContainerName)
            .WithEnvironment("MassTransit__Host", _rabbitMqContainer.IpAddress)
            .WithEnvironment("MassTransit__UserName", "guest")
            .WithEnvironment("MassTransit__Password", "guest")
            .WithEnvironment("EmployeesUrl", "http://host.docker.internal:5189")
            .WithPortBinding(5045, 8080) // TODO: https
            .WithPortBinding(4200, 4200)
            .DependsOn(_rabbitMqContainer)
            .DependsOn(_employeesContainer)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilMessageIsLogged(new Regex(@"Bus started: rabbitmq://\d{1,3}(.\d{1,3}){3}/")))
            .Build();
    }

    private IContainer BuildEmployeesContainer()
    {
        return new ContainerBuilder()
            .WithImage(_employeesImage)
            .WithName(EmployeesContainerName)
            .WithEnvironment("MassTransit__Host", _rabbitMqContainer.IpAddress)
            .WithEnvironment("MassTransit__UserName", "guest")
            .WithEnvironment("MassTransit__Password", "guest")
            .WithEnvironment("Database__ConnectionString", _postgreSqlContainer.GetConnectionString().Replace("127.0.0.1", "host.docker.internal"))
            .WithPortBinding(5189, 8080) // TODO: https
            .DependsOn(_rabbitMqContainer)
            .DependsOn(_postgreSqlContainer)
            .Build();
    }

    private WebDriverContainer BuildWebDriverContainer()
    {
        return new WebDriverBuilder()
            .WithRecording()
            .Build();
    }
}
