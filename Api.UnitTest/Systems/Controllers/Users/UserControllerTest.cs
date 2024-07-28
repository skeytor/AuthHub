﻿using Api.UnitTest.Systems.Controllers.Users.TestsData;
using AuthHub.Api.Controllers;
using AuthHub.Api.Dtos;
using AuthHub.Api.Services.Users;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Api.UnitTest.Systems.Controllers.Users;

public class UserControllerTest
{

    [Theory, ClassData(typeof(UserResponseListValidTestData))]
    public async Task GetAll_Should_ReturnUserResponseList_WhenUsersExist(
        List<UserResponse> usersExpected)
    {
        // Arrange
        var mockUserService = new Mock<IUserService>();
        mockUserService
            .Setup(service => service.GetAllAsync())
            .ReturnsAsync(usersExpected)
            .Verifiable(Times.Once());
        UserController userController = new(mockUserService.Object);

        // Act
        var sutActionResult = await userController.GetAll();

        // Assert
        mockUserService.Verify();
        sutActionResult
            .Should()
            .BeOfType<OkObjectResult>();

        OkObjectResult okObjectResult = (OkObjectResult)sutActionResult;
        okObjectResult.StatusCode
            .Should()
            .Be(StatusCodes.Status200OK);
        okObjectResult.Value
            .Should()
            .BeAssignableTo<IReadOnlyCollection<UserResponse>>();

        var dataResponse = (IReadOnlyCollection<UserResponse>)okObjectResult.Value!;
        dataResponse
            .Should()
            .NotBeEmpty()
            .And
            .HaveSameCount(usersExpected);
    }


    [Theory, ClassData(typeof(UserControllerCreateUserValidTestData))]
    public async Task Create_Should_ReturnUserId_WhenRequestDataIsValid(
        CreateUserRequest request, Guid idExpected)
    {
        // Arrange
        var mockUserService = new Mock<IUserService>();
        mockUserService
            .Setup(service => service.CreateAsync(request))
            .ReturnsAsync(idExpected)
            .Verifiable(Times.Once());

        UserController userController = new(mockUserService.Object);

        // Act
        var sutActionResult = await userController.Create(request);

        // Assert
        mockUserService.Verify();
        sutActionResult
            .Should()
            .BeOfType<CreatedAtActionResult>();

        CreatedAtActionResult createdResult = (CreatedAtActionResult)sutActionResult;
        createdResult.StatusCode
            .Should()
            .Be(StatusCodes.Status201Created);
        createdResult.Value
            .Should()
            .BeOfType<Guid>();

        Guid userId = (Guid)createdResult.Value!;
        userId
            .Should()
            .NotBeEmpty();
    }

    [Theory, ClassData(typeof(UserControllerUserResponseGuidValidTestData))]
    public async Task GetById_Should_ReturnUser_WhenUserExists(Guid id, UserResponse expected)
    {
        // Arrange
        Mock<IUserService> userServiceMock = new();
        userServiceMock
            .Setup(service => service.GetByIdAsync(id))
            .ReturnsAsync(expected)
            .Verifiable(Times.Once());
        UserController userController = new(userServiceMock.Object);

        // Act
        var sutActionResult = await userController.GetById(id);

        // Assert
        sutActionResult.Should().BeOfType<OkObjectResult>();
        OkObjectResult result = (OkObjectResult)sutActionResult;
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Value.Should().BeOfType<UserResponse>();
        var data = (UserResponse)result.Value!;
        data.Should().NotBeNull();
    }
}