using FluentAssertions;
using RabbitMQExamples;
using SeleniumExample.Portal.Server.Dtos;

namespace SeleniumExample.TestsWithWebDriver;

public class Tests : TestsBase
{
    [Test]
    public void ShouldCheckCreationResult()
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
}