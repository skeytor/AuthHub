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
            RoleId: role.Id);

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

    [Fact]
    public async Task GetById_Should_ReturnUserSuccess_WhenUserExists()
    {
        // Arrange
        var role = new Role()
        {
            Name = "admin1",
            Description = "This is admin user"
        };
        var user = new User()
        {
            FirstName = "First Name",
            LastName = "Last Name",
            Email = "email@example1.com",
            Password = "Pass123@",
            Username = "user_name1",
            Role = role
        };
        await _context.Roles.AddAsync(role);
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        Guid id = user.Id;
        
        // Act
        HttpResponseMessage response = await _httpClient.GetAsync($"/api/user/{id}");

        // Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var data = await response.Content.ReadFromJsonAsync<UserResponse>();
        data?.Id.Should().Be(id);
        data?.Name.Should().Be(user.FirstName);
        data?.LastName.Should().Be(user.LastName);
    }

    [Fact]
    public async Task Update_Should_Return200StatusCode_WhenRequestDataIsValid()
    {
        // Arrange
        var role = new Role()
        {
            Name = "admin1",
            Description = "This is admin user"
        };
        var user = new User()
        {
            FirstName = "First Name",
            LastName = "Last Name",
            Email = "email@example2.com",
            Password = "Pass123@",
            Username = "user_name3",
            Role = role
        };
        await _context.Roles.AddAsync(role);
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        CreateUserRequest request = new(
            "Name changed",
            "Last Name changed",
            "user_changed",
            "example_changed@email.com",
            "PassChanged123$",
            RoleId: role.Id);

        // Act
        HttpResponseMessage response = await _httpClient.PutAsJsonAsync(
            $"/api/user/{user.Id}",
            request);
        
        // Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var id = await response.Content.ReadFromJsonAsync<Guid>();
        id.Should().NotBeEmpty();
        var userChanged = await _context.Users.FindAsync(id);
        userChanged?.FirstName.Should().Be(request.FirstName);
        userChanged?.LastName.Should().Be(request.LastName);
        userChanged?.Username.Should().Be(request.UserName);
    }
}
