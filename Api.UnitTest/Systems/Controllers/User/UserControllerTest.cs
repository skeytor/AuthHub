using AuthHub.Api.Controllers;
using AuthHub.Api.Dtos;
using AuthHub.Api.Services.UserService;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Api.UnitTest.Systems.Controllers.User;

public class UserControllerTest
{

    [Theory, ClassData(typeof(SetupUserControllerValidTestData))]
    public async Task GetAll_Should_ReturnUserResponseList_WhenUsersExist(List<UserResponse> fakeUsers)
    {
        // Arrange
        var mockUserService = new Mock<IUserService>();
        mockUserService
            .Setup(service => service.GetAllUsers())
            .ReturnsAsync(fakeUsers)
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
        okObjectResult
            .StatusCode
            .Should()
            .Be(StatusCodes.Status200OK);
        okObjectResult
            .Value
            .Should()
            .BeAssignableTo<IReadOnlyCollection<UserResponse>>();

        var dataResponse = (IReadOnlyCollection<UserResponse>)okObjectResult.Value!;
        dataResponse
            .Should()
            .NotBeEmpty()
            .And
            .HaveSameCount(fakeUsers);
    }


    [Fact]
    public async Task Create_Should_ReturnUserId_WhenRequestDataIsValid()
    {
        // Arrange
        CreateUserRequest request = new(
            "First Name",
            "Last Name",
            "user_name",
            "example@email.com",
            "P@ssword123",
            1
        );
        var mockUserService = new Mock<IUserService>();
        mockUserService
            .Setup(service => service.Create(request))
            .ReturnsAsync(Guid.NewGuid())
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
        createdResult
            .StatusCode
            .Should()
            .Be(StatusCodes.Status201Created);
        createdResult
            .Value
            .Should()
            .BeOfType<Guid>();

        Guid userId = (Guid)createdResult.Value!;
        userId
            .Should()
            .NotBeEmpty();
    }
}