using AuthHub.Api.Controllers;
using AuthHub.Api.Dtos;
using AuthHub.Api.Services.UserService;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NuGet.Frameworks;

namespace AuthHub.UnitTests.Systems.AuthHub.Api.Controllers;

public class UserControllerTests
{

    [Fact]
    public async Task Get_Should_ReturnSuccess_WhenInvokeUserServiceExactlyOnce()
    {
        // Arrange
        var mockUserService = new Mock<IUserService>();
        mockUserService
            .Setup(service => service.GetAllUsers())
            .ReturnsAsync([new(Guid.NewGuid(), Faker.Name.First(), Faker.Name.Last(), Faker.Internet.Email())]);
        var sut = new UserController(mockUserService.Object);

        // Act
        var result = (OkObjectResult)await sut.GetAll();

        // Assert
        mockUserService
            .Verify(service => service.GetAllUsers(), Times.Once());
        Assert.Equal(200, result.StatusCode);
    }

    [Fact]
    public async Task GetAllUsers_Should_ReturnListOfUsers_WhenInvokeUserService()
    {
        // Arrange
        var mockUserService = new Mock<IUserService>();
        mockUserService
            .Setup(service => service.GetAllUsers())
            .ReturnsAsync([new(Guid.NewGuid(), Faker.Name.First(), Faker.Name.Last(), Faker.Internet.Email())]);
        var sut = new UserController(mockUserService.Object);

        // Act
        var result = await sut.GetAll();

        // Assert
        Assert.IsType<OkObjectResult>(result);
        var objectResult = (ObjectResult)result;
        Assert.IsAssignableFrom<IReadOnlyCollection<UserResponse>>(objectResult.Value);
        mockUserService
            .Verify(service => service.GetAllUsers(), Times.Once());
    }

    [Fact]
    public async Task GetAllUsers_Should_ReturnEmptyListOfUsers_WhenInvokeUserService()
    {
        var mockUserService = new Mock<IUserService>();
        mockUserService
            .Setup(service => service.GetAllUsers())
            .ReturnsAsync([]);
        var sut = new UserController(mockUserService.Object);

        // Act
        var result = await sut.GetAll();

        // Assert
        Assert.IsType<NotFoundResult>(result);
        mockUserService
            .Verify(service => service.GetAllUsers(), Times.Once());
    }
}
