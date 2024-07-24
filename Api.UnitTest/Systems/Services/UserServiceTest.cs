using AuthHub.Api.Dtos;
using AuthHub.Api.Services.UserService;
using AuthHub.Domain.Entities;
using AuthHub.Domain.Repositories;
using AuthHub.Domain.Results;
using AuthHub.Persistence.Abstractions;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Api.UnitTest.Systems.Services;

public class UserServiceTest
{
    [Theory, ClassData(typeof(SetupUserServiceValidUsersTestData))]
    public async Task GetAllAsync_Should_ReturnUserList_WhenUsersExist(IReadOnlyList<User> expected)
    {
        // Arrange
        Mock<IUserRepository> mockUserRepository = new();

        mockUserRepository
            .Setup(repository => repository.GetAllAsync())
            .ReturnsAsync(expected)
            .Verifiable(Times.Once());
        UserService userService = new(mockUserRepository.Object, default!, default!);

        // Act
        var result = await userService.GetAllAsync();

        // Assert
        mockUserRepository.Verify();
        result.IsSuccess
            .Should()
            .BeTrue();
        result.Value
            .Should()
            .BeAssignableTo<IReadOnlyList<UserResponse>>();
        result.Value
            .Should()
            .NotBeEmpty()
            .And
            .HaveSameCount(expected);
    }

    [Fact]
    public async Task CreateAsync_Should_ReturnId_WhenUserDoesNotExist()
    {
        // Arrange
        CreateUserRequest request = new(
            "First Name",
            "Last Name",
            "user_name",
            "example@email.com",
            "Pass123", RoleId: 1);
        User expected = new()
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Username = request.UserName,
            Email = request.Email,
            RoleId = request.RoleId,
            IsActive = true
        };

        Mock<IUserRepository> mockUserRepository = new();
        Mock<IUnitOfWork> mockUnitOfWork = new();
        Mock<IPasswordHasher<User>> mockPasswordHasher = new();

        mockPasswordHasher.Setup(provider => provider.HashPassword(It.IsAny<User>(), request.Password))
            .Returns(It.IsAny<string>())
            .Verifiable(Times.Once());

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
            .ReturnsAsync(1) // Simulate a successfull save operation affecting 1 row
            .Verifiable(Times.Once());

        UserService userService = new(
            mockUserRepository.Object,
            mockUnitOfWork.Object,
            mockPasswordHasher.Object);

        // Act
        var result = await userService.CreateAsync(request);

        // Assert
        mockUserRepository.Verify();
        mockUnitOfWork.Verify();
        mockPasswordHasher.Verify();
        result.IsSuccess
            .Should()
            .BeTrue();
        result.Error
            .Should()
            .Be(Error.None);
        result.Value
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
            .Verifiable(Times.Never()); // It should never be called

        mockUnitOfWork
            .Setup(unitOfWork => unitOfWork.SaveChangesAsync(default))
            .ReturnsAsync(0) // 0 rows affected
            .Verifiable(Times.Never()); // It should never be called

        UserService userService = new(mockUserRepository.Object, mockUnitOfWork.Object, default!);

        // Act
        var result = await userService.CreateAsync(request);

        // Assert
        mockUserRepository.Verify();
        mockUnitOfWork.Verify();
        result.IsFailure
            .Should()
            .BeTrue();
        result.Error.Type
            .Should()
            .Be(ErrorType.Conflict);
    }

    [Theory, ClassData(typeof(SetupUserServiceGuidUserTestData))]
    public async Task GetUserById_Should_ReturnUser_WhenUserExists(Guid id, User userExpected)
    {
        // Arrange
        Mock<IUserRepository> mockUserRepository = new();
        mockUserRepository
            .Setup(repo => repo.GetByIdAsync(id))
            .ReturnsAsync(userExpected)
            .Verifiable(Times.Once());
        UserService userService = new(mockUserRepository.Object, default!, default!);

        // Act
        var result = await userService.GetByIdAsync(id);

        // Assert
        mockUserRepository.Verify();
        result.IsSuccess
            .Should()
            .BeTrue();
        result.Value
            .Should()
            .BeOfType<UserResponse>();
        result.Value
            .Should()
            .NotBeNull();
        result.Value.LastName
            .Should()
            .Be(userExpected.LastName);
    }

}
