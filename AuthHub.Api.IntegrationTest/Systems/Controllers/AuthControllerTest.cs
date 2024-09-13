using AuthHub.Api.Dtos;
using AuthHub.Api.IntegrationTest.Fixtures;
using AuthHub.Domain.Entities;
using AuthHub.Persistence;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;

namespace AuthHub.Api.IntegrationTest.Systems.Controllers;

[Collection(nameof(WebApplicationCollectionFixture))]
public class AuthControllerTest(IntegrationTestWebApplicationFactory<Program> fixture)
    : BaseWebApplicationTest(fixture)
{
    [Fact]
    public async Task GetAccessToken_Should_ReturnSuccess_WhenCredentialsAreValidAndUserExists()
    {
        // Arrange
        Role role = new()
        {
            Name = "Admin",
            Description = "This is an admin user"
        };

        using var scope = _factory.Services.CreateScope();
        AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        context.Roles.Add(role);
        context.SaveChanges();

        await _httpClient.PostAsJsonAsync("/api/user",
            new CreateUserRequest("Test1", "Test1", "user_name1", "test1@email.com", "Password12!", 1));

        LoginRequest request = new("user_name1", "Password12!");

        // Act
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("/api/auth/login", request);

        // Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var tokenValue = await response.Content.ReadFromJsonAsync<AccessTokenResponse>();
        tokenValue.Should().NotBeNull();
        var decodedToken = new JwtSecurityTokenHandler().ReadJwtToken(tokenValue?.AccessToken);
    }

    [Fact]
    public async Task GetAccessToken_Should_ReturnError_WhenCredentialsAreInvalid()
    {
        // Arrange
        Role role = new()
        {
            Name = "Admin",
            Description = "This is an admin user"
        };

        using var scope = _factory.Services.CreateScope();
        AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        context.Roles.Add(role);
        context.SaveChanges();

        await _httpClient.PostAsJsonAsync("/api/user",
            new CreateUserRequest("Test1", "Test1", "user_name1", "test1@email.com", "Password12!", 1));

        LoginRequest request = new("user_name1", "Password122!");

        // Act
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("/api/auth/login", request);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }
}
