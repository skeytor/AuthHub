using AuthHub.Api.Dtos;
using AuthHub.Api.Services.UserService;
using AuthHub.Domain.Repositories;
using Moq;

namespace Api.UnitTest.Services;

public class UserServiceTest
{
    [Fact]
    public async Task GetAllAsync_Should_ReturnUserEntityList_WhenInvokeUserRepository()
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
