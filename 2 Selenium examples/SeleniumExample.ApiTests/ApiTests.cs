using FluentAssertions;
using SeleniumExample.Portal.Server.Dtos;

namespace RabbitMQExamples;

public class ApiTests: TestsBase
{
    [Test]
    public async Task CreateEmployee_CorrectRequestProvided_ShouldCreateEmployee()
    {
        // Arrange
        var request = new CreateEmployeeRequest("+12345678900", "userName1", "test@example.com", "Инженер-программист");;

        // Act
        var createResult = await PortalService.CreateEmployeeAsync(request);
        var getResult = await PortalService.GetEmployeeAsync(createResult.Id);

        // Assert
        getResult.Should().Be(new GetEmployeeDto(
            createResult.Id,
            request.Phone,
            request.UserName,
            request.Email,
            request.Position));
    }
}