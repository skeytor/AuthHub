using AuthHub.Api.Dtos;
using AuthHub.Domain.Entities;
using FluentAssertions;
using System.Net.Http.Json;

namespace AuthHub.Api.IntegrationTest;

[Collection(nameof(WebApplicationCollectionFixture))]
public class UserControllerTests(IntegrationTestWebApplicationFactory<Program> factory)
    : BaseWebApplicationTest(factory)
{
    [Fact]
    public async Task GetAllUsers_Should_ReturnSuccess()
    {
        // Arrange

        // Act
        HttpResponseMessage response = await _httpClient.GetAsync("/api/user");

        // Assertions
        response.EnsureSuccessStatusCode();
        var userResponse = await response
            .Content
            .ReadFromJsonAsync<IReadOnlyCollection<UserResponse>>();
        userResponse
            .Should()
            .BeAssignableTo<IReadOnlyCollection<UserResponse>>();
    }

    [Fact]
    public async Task CreateUser_Should_ReturnSuccess_WhenUserDoesNotExist()
    {
        // Arrange
        var role = new Role()
        {
            Name = "admin",
            Description = "This is admin user",
        };
        await _context.Roles.AddAsync(role);

        var request = new CreateUserRequest(
            "First Name",
            "Last Name",
            "user_name",
            "example@email.com",
            "Rober123",
            RoleId: 1);

        // Act
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("/api/user", request);

        // Assertions
        response.EnsureSuccessStatusCode();

    }
}
