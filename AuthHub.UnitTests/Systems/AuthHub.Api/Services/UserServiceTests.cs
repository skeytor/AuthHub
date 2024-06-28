using AuthHub.Api.Dtos;
using AuthHub.Api.Services.UserService;
using AuthHub.Domain.Repositories;
using Moq;

namespace AuthHub.UnitTests.Systems.AuthHub.Api.Services;

public class UserServiceTests
{
    [Fact]
    public async Task GetAllUsers_Should_ReturnUserList_WhenInvokeUserRepository()
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
}
