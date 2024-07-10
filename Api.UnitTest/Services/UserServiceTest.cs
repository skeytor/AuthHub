using AuthHub.Api.Dtos;
using AuthHub.Api.Services.UserService;
using AuthHub.Domain.Entities;
using AuthHub.Domain.Repositories;
using FluentAssertions;
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

    [Fact]
    public async Task CreateAsync_Should_ReturnId_WhenInvokeCreateUserRepositoryMethod()
    {
        // Arrange
        UserRequest userRequest = new("Pepito", "Leon", "rleon", "example@email.com", "Pass123", 1);
        User expectedUser = new()
        {
            Id = Guid.NewGuid(),
            FirstName = userRequest.FirstName,
            LastName = userRequest.LastName,
            Username = userRequest.UserName,
            Email = userRequest.Email,
            Password = userRequest.Password,
            RoleId = userRequest.RoleId,
            IsActive = true
        };
        Mock<IUserRepository> mockUserRepository = new();
        mockUserRepository
            .Setup(repo => repo.CreateAsync(It.IsAny<User>()))
            .ReturnsAsync(expectedUser)
            .Verifiable(Times.Once());
        UserService sut = new(mockUserRepository.Object);

        // Act
        var result = await sut.Create(userRequest);
        
        // Assert
        mockUserRepository.Verify();
        result.Should().NotBe(Guid.Empty); // Assertions with FluentAssertions
        result.Should().Be(expectedUser.Id);
    }
}
