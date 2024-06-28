using AuthHub.Api.Dtos;
using AuthHub.Api.Services.UserService;
using AuthHub.Domain.Entities;
using AuthHub.Domain.Repositories;
using AuthHub.UnitTests.Systems.AuthHub.Persistence.Repositories;
using Moq;

namespace AuthHub.UnitTests.Systems.AuthHub.Api.Services;

public class UserServiceTests
{
    [Fact]
    public async Task GetAllUsers_Should_ReturnEmptyListOfUsers_WhenInvokeUserRepository()
    {
        // Arrange
        var mockUserRepository = new Mock<IUserRepository>();
        mockUserRepository
            .Setup(repository => repository.GetAllAsync())
            .ReturnsAsync([]);
        var sut = new UserService(mockUserRepository.Object);

        // Act
        var result = await sut.GetAllUsers();

        // Assert
        Assert.IsAssignableFrom<IReadOnlyCollection<UserResponse>>(result);
        mockUserRepository.Verify(repository => repository.GetAllAsync(), Times.Once());
    }

    [Theory, ClassData(typeof(UserDataTest))]
    public async Task GetAllUsers_Should_ReturnListOfUsers_WhenInvokeUserRepository(List<User> users)
    {
        // Arrange
        var mockUserRepository = new Mock<IUserRepository>();
        mockUserRepository
            .Setup(repository => repository.GetAllAsync())
            .ReturnsAsync(users);
        var sut = new UserService(mockUserRepository.Object);

        // Act
        var result = await sut.GetAllUsers();

        // Assert
        Assert.IsAssignableFrom<IReadOnlyCollection<UserResponse>>(result);
        Assert.NotEmpty(result);
        Assert.Equal(users.Count, result.Count);
        mockUserRepository.Verify(repository => repository.GetAllAsync(), Times.Once());
    }
}
