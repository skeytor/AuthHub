using AuthHub.Api.Dtos;
using AuthHub.Api.IntegrationTest.Fixtures;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication.BearerToken;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using Xunit.Abstractions;

namespace AuthHub.Api.IntegrationTest.Systems.Controllers;

[Collection(nameof(WebApplicationCollectionFixture))]
public class AuthControllerTest(
    IntegrationTestWebApplicationFactory<Program> fixture,
    ITestOutputHelper outputHelper)
    : BaseWebApplicationTest(fixture, outputHelper)
{
    [Fact]
    public async Task GetAccessToken_Should_ReturnSuccess_WhenCredentialsAreValidAndUserExists()
    {
        // Arrange
        LoginRequest request = new("rober_1", "StrongPassword!12"); // See SampleData.Users

        // Act
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("/api/auth/login", request);

        // Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var tokenValue = await response.Content.ReadFromJsonAsync<AccessTokenResponse>();
        tokenValue.Should().NotBeNull();
        JwtSecurityToken decodedToken = new JwtSecurityTokenHandler()
            .ReadJwtToken(tokenValue?.AccessToken);
        decodedToken.Claims.Any().Should().BeTrue();
        decodedToken.Claims.Should().Contain(x => x.Type == JwtRegisteredClaimNames.Sub);
        
    }

    [Fact]
    public async Task GetAccessToken_Should_ReturnError_WhenCredentialsAreInvalid()
    {
        // Arrange
        LoginRequest request = new("invalid_username", "InvalidPassword12!");

        // Act
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("/api/auth/login", request);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }
}
