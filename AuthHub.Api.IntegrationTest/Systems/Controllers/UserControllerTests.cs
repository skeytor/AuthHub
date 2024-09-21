﻿using AuthHub.Api.Dtos;
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
        var data = await response
            .Content
            .ReadFromJsonAsync<IReadOnlyCollection<UserResponse>>();
        data.Should()
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
        using IServiceScope scope = _factory.Services.CreateScope();
        AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var user = await context.Users.FindAsync(userId);
        user.Should().NotBeNull();
        user.Id.Should().Be(userId);
        // check others properties if you want...
    }

    [Fact]
    public async Task GetById_Should_ReturnUserSuccess_WhenUserExists()
    {
        // Arrange
        using IServiceScope scope = _factory.Services.CreateScope();
        AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        Guid userId = context.Users
            .OrderBy(x => Guid.NewGuid())
            .Select(x => x.Id)
            .FirstOrDefault();

        // Act
        HttpResponseMessage response = await _httpClient.GetAsync($"/api/user/{userId}");

        // Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        UserResponse? data = await response.Content.ReadFromJsonAsync<UserResponse>();
        data.Should().NotBeNull();
    }

    [Fact]
    public async Task Update_Should_Return200StatusCode_WhenRequestDataIsValid()
    {
        // Arrange
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
            $"/api/user/{userId}",
            request);

        // Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        Guid id = await response.Content.ReadFromJsonAsync<Guid>();
        id.Should().NotBeEmpty();
    }
}
