using ApiTestsExamples.Infrastructure;
using FluentAssertions;

namespace ApiTestsExamples;

[Collection(nameof(ApiTestsCollection))]
public class ApiTests
{
    private readonly ApiTestsFixture _fixture;

    public ApiTests(ApiTestsFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task ShouldGetSuccessfulResponse()
    {
        // Arrange
        var httpClient = new HttpClient();
        var uri = _fixture.GetUri();

        // Act
        var response = await httpClient.GetAsync(uri);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
    }
}
