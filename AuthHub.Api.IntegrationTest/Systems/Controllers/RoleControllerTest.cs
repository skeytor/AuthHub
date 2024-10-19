using AuthHub.Api.Dtos;
using AuthHub.Api.IntegrationTest.Fixtures;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using Xunit.Abstractions;

namespace AuthHub.Api.IntegrationTest.Systems.Controllers;

[Collection(nameof(WebApplicationCollectionFixture))]
public class RoleControllerTest(
    IntegrationTestWebApplicationFactory<Program> fixuture,
    ITestOutputHelper outputHelper) 
    : BaseWebApplicationTest(fixuture, outputHelper)
{
    [Theory]
    [InlineData("/api/role")]
    public async Task GetRoles_Should_ReturnRolesList(string pathURL)
    {
        // Arrange
        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(pathURL);

        // Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var data = await response
            .Content
            .ReadFromJsonAsync<IReadOnlyList<RoleResponse>>();
        data.Should().Contain(x => x.Name == "Admin");
    }

    [Fact]
    public async Task CreateRole_Should_ReturnRoleName_WhenRoleDoesNotExist()
    {
        // Arrange
        CreateRoleRequest request = new("Role Test", "This is an administrator", Permissions: [1, 2, 3]);

        // Act
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("/api/role", request);

        // Assert
        string message = await response.Content.ReadAsStringAsync();
        _testOutputHelper.WriteLine(message);

        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var data = await response.Content.ReadAsStringAsync();
        data.Should().Be(request.Name);
    }

    [Fact]
    public async Task CreateRole_Should_ReturnFailure_WhenRoleExists()
    {
        // Arrange
        CreateRoleRequest request = new("Admin", "This is a description test", []); // See SampleData.Roles

        // Act
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("/api/role", request);

        // Assert
        string message = await response.Content.ReadAsStringAsync();
        _testOutputHelper.WriteLine($"Response Message: {message}");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();

    }
}
