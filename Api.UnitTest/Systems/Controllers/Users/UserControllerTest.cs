using Api.UnitTest.Systems.Controllers.Users.TestsData;
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

    [Theory, ClassData(typeof(ValidUserResponseListTestData))]
    public async Task GetAll_Should_ReturnUserList_WhenUsersExist(
        List<UserResponse> expectedResult)
    {
        // Arrange
        var mockUserService = new Mock<IUserService>();
        mockUserService
            .Setup(service => service.GetAllAsync())
            .ReturnsAsync(expectedResult)
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
            .HaveSameCount(expectedResult);
    }


    [Theory, ClassData(typeof(UserControllerCreateUserValidTestData))]
    public async Task Create_Should_ReturnUserId_WhenRequestDataIsValid(
        CreateUserRequest input, Guid expectedResult)
    {
        // Arrange
        var mockUserService = new Mock<IUserService>();
        mockUserService
            .Setup(service => service.RegisterAsync(input))
            .ReturnsAsync(expectedResult)
            .Verifiable(Times.Once());

        UserController userController = new(mockUserService.Object);

        // Act
        var sutActionResult = await userController.Register(input);

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

    [Theory, ClassData(typeof(UserControllerUserResponseValidTestData))]
    public async Task GetById_Should_ReturnUser_WhenUserExists(Guid id, UserResponse expectedResult)
    {
        // Arrange
        Mock<IUserService> userServiceMock = new();
        userServiceMock
            .Setup(service => service.GetByIdAsync(id))
            .ReturnsAsync(expectedResult)
            .Verifiable(Times.Once());
        UserController userController = new(userServiceMock.Object);

        // Act
        var sutActionResult = await userController.GetById(id);

        // Assert
        sutActionResult
            .Should()
            .BeOfType<OkObjectResult>();
        OkObjectResult result = (OkObjectResult)sutActionResult;
        result.StatusCode
            .Should()
            .Be(StatusCodes.Status200OK);
        result.Value
            .Should()
            .BeOfType<UserResponse>();
        var data = (UserResponse)result.Value!;
        data.Should().NotBeNull();
    }

    [Fact]
    public async Task Update_Should_ReturnUserId_WhenUserExists()
    {
        // Arrange
        Guid id = Guid.NewGuid();
        CreateUserRequest request = new(
            "Name changed",
            "Last name changed",
            "Username changed",
            "example_changed@email.com",
            "Pas12Changed",
            RoleId: 4);
        Mock<IUserService> mockUserServiceMock = new();
        mockUserServiceMock
            .Setup(service => 
                service.Update(It.IsAny<Guid>(), It.IsAny<CreateUserRequest>()))
            .ReturnsAsync(id)
            .Verifiable(Times.Once());
        UserController userController = new(mockUserServiceMock.Object);

        // Act
        var sutActionResult = await userController.Update(id, request);

        // Asseert
        mockUserServiceMock.Verify();
        sutActionResult.Should().BeOfType<OkObjectResult>();
        OkObjectResult result = (OkObjectResult)sutActionResult;
        result.StatusCode
            .Should()
            .Be(StatusCodes.Status200OK);
        result.Value.Should().BeOfType<Guid>();
        var guid = (Guid)result.Value!;
        guid.Should().NotBeEmpty();
    }
}