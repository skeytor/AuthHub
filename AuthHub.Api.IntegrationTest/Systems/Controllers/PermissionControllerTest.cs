using AuthHub.Api.IntegrationTest.Fixtures;
using AuthHub.Domain.Entities;
using AuthHub.Infrastructure.Authorization;
using FluentAssertions;
using System.Net.Http.Json;
using Xunit.Abstractions;
namespace AuthHub.Api.IntegrationTest.Systems.Controllers;

[Collection(nameof(WebApplicationCollectionFixture))]
public class PermissionControllerTest(
    IntegrationTestWebApplicationFactory<Program> fixture,
    ITestOutputHelper testOutputHelper) 
    : BaseWebApplicationTest(fixture, testOutputHelper)
{
    [Theory]
    [InlineData("/api/permission")]
    public async Task GetAllPermissions_Should_Return200OKStatusCode(string pathURL)
    {
        // Arrange
        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(pathURL);

        // Assert
        string message = await response.Content.ReadAsStringAsync();
        _testOutputHelper.WriteLine($"Message: {message}");

        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var data = await response.Content.ReadFromJsonAsync<IReadOnlyList<Permission>>();

        data.Should().NotBeNull();
        data.Should().NotContain(x => x.Name == nameof(Permissions.None) && 
                                      x.Name == nameof(Permissions.All));
    }
}
