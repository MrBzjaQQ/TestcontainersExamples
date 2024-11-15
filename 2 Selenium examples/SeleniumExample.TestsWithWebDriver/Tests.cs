using FluentAssertions;
using RabbitMQExamples;
using SeleniumExample.Portal.Server.Dtos;
using System.Text.Json;

namespace SeleniumExample.TestsWithWebDriver;

public class Tests : TestsBase
{
    [Test]
    public void ShouldCheckCreateResult()
    {
        // Arrange
        var createEmployeeRequest = new CreateEmployeeRequest("+71111111111", "testUser", "test@example.com", "Инженер-программист");
        PageObject.OpenPage("http://host.docker.internal:5045/");

        // Act
        PageObject.Form.FillForm(createEmployeeRequest);
        PageObject.Form.SubmitForm();
        PageObject.WaitForRequestsEnd();

        // Assert
        var response = PageObject.GetCreateResult();
        response.Should().NotBeNull();
    }

    private record GetEmployeeResponse
    {
        public string Id { get; init; } = string.Empty;
        public string Phone { get; init; } = string.Empty;
        public string UserName { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string Position { get; init; } = string.Empty;
    }
}