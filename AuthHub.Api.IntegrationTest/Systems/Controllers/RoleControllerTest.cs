using AuthHub.Api.Dtos;
using AuthHub.Api.IntegrationTest.Fixtures;
using AuthHub.Persistence;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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

    [Theory]
    [InlineData("/api/role")]
    public async Task CreateRole_Should_ReturnRoleName_WhenRoleDoesNotExist(string pathURL)
    {
        // Arrange
        CreateRoleRequest request = new("Role Test", "This is an administrator", Permissions: [1, 2, 3]);

        // Act
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(pathURL, request);

        // Assert
        string message = await response.Content.ReadAsStringAsync();
        _testOutputHelper.WriteLine(message);

        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var data = await response.Content.ReadAsStringAsync();
        data.Should().Be(request.Name);
    }

    [Theory]
    [InlineData("/api/role")]
    public async Task CreateRole_Should_ReturnFailure_WhenRoleExists(string pathURL)
    {
        // Arrange
        CreateRoleRequest request = new("Admin", "This is a description test", []); // See SampleData.Roles

        // Act
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(pathURL, request);

        // Assert
        string message = await response.Content.ReadAsStringAsync();
        _testOutputHelper.WriteLine($"Response Message: {message}");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
    }

    [Theory]
    [InlineData("/api/role")]
    public async Task UpdateRole_Should_ReturnNoContentStatusCode(string pathURL)
    {
        // Arrange
        CreateRoleRequest request = new("AdminChanged", "This role was changed", [1, 2]);
        int roleId = 1;

        // Act
        HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"{pathURL}/{roleId}", request);

        // Assert
        string message = await response.Content.ReadAsStringAsync();
        _testOutputHelper.WriteLine($"Message: {message}");

        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        string responseData = await response.Content.ReadAsStringAsync();
        responseData.Should().Be(request.Name);

        using IServiceScope scope = _factory.Services.CreateScope();
        AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        
        var roleUpdated = context.Roles
            .Include(x => x.Permissions)
            .FirstOrDefault(x => x.Id == roleId);
        roleUpdated!.Permissions.Should().HaveSameCount(request.Permissions);
        roleUpdated!.Permissions.Should().Contain(x => request.Permissions.Contains(x.Id));
    }
}
