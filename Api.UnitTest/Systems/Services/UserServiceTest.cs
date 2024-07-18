using Api.UnitTest.Enums;
using AuthHub.Api.Dtos;
using AuthHub.Api.Services.UserService;
using AuthHub.Domain.Entities;
using AuthHub.Domain.Repositories;
using AuthHub.Domain.Results;
using AuthHub.Persistence.Abstractions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;

namespace Api.UnitTest.Systems.Services;

public class UserServiceTest
{
    [Theory, ClassData(typeof(SetupUserServiceValidTestData))]
    public async Task GetAllAsync_Should_ReturnUserList_WhenUsersExist(IReadOnlyCollection<User> expected)
    {
        // Arrange
        Mock<IUserRepository> mockUserRepository = new();
        Mock<IUnitOfWork> mockUnitOfWork = new();
        mockUserRepository
            .Setup(repository => repository.GetAllAsync())
            .ReturnsAsync(expected)
            .Verifiable(Times.Once());
        UserService userService = new(mockUserRepository.Object, mockUnitOfWork.Object);

        // Act
        var result = await userService.GetAllUsers();

        // Assert
        mockUserRepository.Verify();
        result
            .IsSuccess
            .Should()
            .BeTrue();
        result
            .Value
            .Should()
            .BeAssignableTo<IReadOnlyCollection<UserResponse>>();
        result
            .Value
            .Should()
            .NotBeEmpty()
            .And
            .HaveSameCount(expected);
    }

    [Fact]
    public async Task CreateAsync_Should_ReturnId_WhenUserDoesNotExist()
    {
        // Arrange
        CreateUserRequest request = new("Pepito", "Leon", "rleon", "example@email.com", "Pass123", RoleId: 1);
        User expected = new()
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Username = request.UserName,
            Email = request.Email,
            Password = request.Password,
            RoleId = request.RoleId,
            IsActive = true
        };
        Mock<IUserRepository> mockUserRepository = new();
        Mock<IUnitOfWork> mockUnitOfWork = new();
        mockUserRepository
            .Setup(repo => repo.ExistAsync(It.IsAny<string>()))
            .ReturnsAsync(false)
            .Verifiable(Times.Once());
        mockUserRepository
            .Setup(repo => repo.CreateAsync(It.IsAny<User>()))
            .ReturnsAsync(expected)
            .Verifiable(Times.Once());
        mockUnitOfWork
            .Setup(unitOfWork => unitOfWork.SaveChangesAsync(default))
            .ReturnsAsync(1) // Simulate a successful save operation affecting 1 row
            .Verifiable(Times.Once());

        UserService userService = new(mockUserRepository.Object, mockUnitOfWork.Object);

        // Act
        var result = await userService.Create(request);

        // Assert
        mockUserRepository.Verify();
        mockUnitOfWork.Verify();
        result
            .IsSuccess
            .Should()
            .BeTrue();
        result
            .Error
            .Should()
            .Be(Error.None);
        result
            .Value
            .Should()
            .NotBeEmpty()
            .And
            .Be(expected.Id);
    }

    [Fact]
    public async Task CreateAsync_Should_ReturnErrorFailure_WhenUserExists()
    {
        // Arrange
        CreateUserRequest request = new("Pepito", "Leon", "rleon", "example@email.com", "Pass123", RoleId: 1);
        User expected = new()
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Username = request.UserName,
            Email = request.Email,
            Password = request.Password,
            RoleId = request.RoleId,
            IsActive = true
        };
        Mock<IUserRepository> mockUserRepository = new();
        Mock<IUnitOfWork> mockUnitOfWork = new();
        mockUserRepository
            .Setup(repo => repo.ExistAsync(It.IsAny<string>()))
            .ReturnsAsync(true)
            .Verifiable(Times.Once());
        mockUserRepository
            .Setup(repo => repo.CreateAsync(It.IsAny<User>()))
            .ReturnsAsync(It.IsAny<User>())
            .Verifiable(Times.Never());
        mockUnitOfWork
            .Setup(unitOfWork => unitOfWork.SaveChangesAsync(default))
            .ReturnsAsync(0) // 0 rows affected
            .Verifiable(Times.Never()); // It never should be called

        UserService userService = new(mockUserRepository.Object, mockUnitOfWork.Object);

        // Act
        var result = await userService.Create(request);

        // Assert
        mockUserRepository.Verify();
        mockUnitOfWork.Verify();
        result
            .IsFailure
            .Should()
            .BeTrue();
        result
            .Error
            .Type
            .Should()
            .Be(ErrorType.Conflict);
    }
}
