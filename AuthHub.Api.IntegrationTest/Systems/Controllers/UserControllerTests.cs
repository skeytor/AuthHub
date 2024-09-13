using AuthHub.Api.Dtos;
using AuthHub.Api.IntegrationTest.Fixtures;
using AuthHub.Infrastructure.Authentication;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace AuthHub.Api.IntegrationTest.Systems.Controllers;

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

        // Assert
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
        // check others properties if you want...
    }

    [Fact]
    public async Task GetById_Should_ReturnUserSuccess_WhenUserExists()
    {
        // Arrange


        // Act
        HttpResponseMessage response = await _httpClient.GetAsync($"/api/user/{1}");

        // Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var data = await response.Content.ReadFromJsonAsync<UserResponse>();
    }

    [Fact]
    public async Task Update_Should_Return200StatusCode_WhenRequestDataIsValid()
    {
        // Arrange

        CreateUserRequest request = new(
            "Name changed",
            "Last Name changed",
            "user_changed",
            "example_changed@email.com",
            "PassChanged123$",
            RoleId: 1);

        // Act
        HttpResponseMessage response = await _httpClient.PutAsJsonAsync(
            $"/api/user/{1}",
            request);

        // Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        Guid id = await response.Content.ReadFromJsonAsync<Guid>();
        id.Should().NotBeEmpty();
    }
}
