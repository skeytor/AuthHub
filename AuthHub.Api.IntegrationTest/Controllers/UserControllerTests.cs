using AuthHub.Api.Dtos;
using AuthHub.Api.IntegrationTest.Fixtures;
using AuthHub.Domain.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace AuthHub.Api.IntegrationTest.Controllers;

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
        response.StatusCode
            .Should()
            .Be(System.Net.HttpStatusCode.OK);
        var userResponse = await response
            .Content
            .ReadFromJsonAsync<IReadOnlyCollection<UserResponse>>();
        userResponse
            .Should()
            .BeAssignableTo<IReadOnlyCollection<UserResponse>>();
    }

    [Fact]
    public async Task CreateUser_Should_ReturnValidationViewModelError_WhenCreateUserRequestIsInvalid()
    {
        // Arrange
        CreateUserRequest request = new("", "", "", "", "", RoleId: 1); // invalid request data.

        // Act
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("/api/user", request);

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
        await _context.SaveChangesAsync();

        CreateUserRequest request = new(
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
        response.StatusCode
            .Should()
            .Be(System.Net.HttpStatusCode.Created);

        Guid userId = await response.Content.ReadFromJsonAsync<Guid>();
        userId.Should().NotBeEmpty();

        var userCreated = await _context.Users.FindAsync(userId);
        
        userCreated.Should().NotBeNull();
        userCreated!.Id.Should().Be(userId);
        userCreated!.Username.Should().Be(request.UserName);
        // check others properties if you want...
    }
}
