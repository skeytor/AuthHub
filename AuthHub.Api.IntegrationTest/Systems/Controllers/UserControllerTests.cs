using AuthHub.Api.Dtos;
using AuthHub.Api.IntegrationTest.Fixtures;
using AuthHub.Persistence;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using Xunit.Abstractions;

namespace AuthHub.Api.IntegrationTest.Systems.Controllers;

[Collection(nameof(WebApplicationCollectionFixture))]
public class UserControllerTests(
    IntegrationTestWebApplicationFactory<Program> factory,
    ITestOutputHelper outputHelper)
    : BaseWebApplicationTest(factory, outputHelper)
{
    [Theory]
    [InlineData("/api/user")]
    public async Task GetAllUsers_Should_ReturnSuccess(string path)
    {
        // Arrange
        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(path);

        // Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode
            .Should()
            .Be(System.Net.HttpStatusCode.OK);
        var data = await response
            .Content
            .ReadFromJsonAsync<IReadOnlyCollection<UserResponse>>();
        data.Should()
            .BeAssignableTo<IReadOnlyCollection<UserResponse>>();
    }

    [Theory]
    [InlineData("/api/user")]
    public async Task CreateUser_Should_ReturnValidationViewModelError_WhenCreateUserRequestIsInvalid(
        string path)
    {
        // Arrange
        CreateUserRequest request = new("", "", "", "", "", RoleId: 1); // invalid request data.

        // Act
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(path, request);

        // Assert
        response.StatusCode
            .Should()
            .Be(System.Net.HttpStatusCode.BadRequest);
        var responseContent = await response
            .Content
            .ReadFromJsonAsync<ProblemDetails>();
        responseContent!.Extensions.Keys
            .Should()
            .NotBeNullOrEmpty()
            .And
            .Contain("errors"); // verify if there is some validation error.
    }

    [Theory]
    [InlineData("/api/user")]
    public async Task CreateUser_Should_ReturnSuccess_WhenUserDoesNotExist(string path)
    {
        // Arrange
        CreateUserRequest request = new(
            "First Name",
            "Last Name",
            "user_name",
            "example@email.com",
            "Rober123",
            RoleId: 1);

        // Act
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(path, request);

        // Assertions
        response.EnsureSuccessStatusCode();
        response.StatusCode
            .Should()
            .Be(System.Net.HttpStatusCode.Created);

        Guid userId = await response.Content.ReadFromJsonAsync<Guid>();
        using IServiceScope scope = _factory.Services.CreateScope();
        AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var user = await context.Users.FindAsync(userId);
        user.Should().NotBeNull();
        user?.Id.Should().Be(userId);
        // check others properties if you want...
    }

    [Theory]
    [InlineData("/api/user")]
    public async Task GetById_Should_ReturnUserSuccess_WhenUserExists(string path)
    {
        // Arrange
        using IServiceScope scope = _factory.Services.CreateScope();
        AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        Guid userId = context.Users
            .OrderBy(x => Guid.NewGuid())
            .Select(x => x.Id)
            .FirstOrDefault();

        // Act
        HttpResponseMessage response = await _httpClient.GetAsync($"{path}/{userId}");

        // Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        UserResponse? data = await response.Content.ReadFromJsonAsync<UserResponse>();
        data.Should().NotBeNull();
    }

    [Theory]
    [InlineData("/api/user")]
    public async Task GetById_Should_ReturnNotFoundStatusCode_WhenUserDoesNotExist(string path)
    {
        // Arrange
        Guid userId = Guid.NewGuid();

        // Act
        HttpResponseMessage response = await _httpClient.GetAsync($"{path}/{userId}");

        // Assert
        string message = await response.Content.ReadAsStringAsync();
        _testOutputHelper.WriteLine(message);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    [Theory]
    [InlineData("/api/user/me")]
    public async Task GetUserProfile_Should_ReturnProfile_WhenUserIsOnlyAuthenticated(string path)
    {
        // Arrange
        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(path);

        // Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Theory]
    [InlineData("/api/user")]
    public async Task Update_Should_Return200StatusCode_WhenRequestDataIsValid(string path)
    {
        // Arrange
        using IServiceScope scope = _factory.Services.CreateScope();
        AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        Guid userId = context.Users
            .OrderBy(x => Guid.NewGuid())
            .Select(x => x.Id)
            .FirstOrDefault();
        CreateUserRequest request = new(
            "Name changed",
            "Last Name changed",
            "user_changed",
            "example_changed@email.com",
            "PassChanged123$",
            RoleId: 1);

        // Act
        HttpResponseMessage response = await _httpClient.PutAsJsonAsync(
            $"{path}/{userId}",
            request);

        // Assert
        string message = await response.Content.ReadAsStringAsync();
        _testOutputHelper.WriteLine($"Message: ", message);

        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        Guid id = await response.Content.ReadFromJsonAsync<Guid>();
        id.Should().NotBeEmpty();
    }
}
