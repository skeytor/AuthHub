using Api.UnitTest.Systems.Services.Users.TestData;
using AuthHub.Api.Dtos;
using AuthHub.Api.Services.Users;
using AuthHub.Domain.Entities;
using AuthHub.Domain.Repositories;
using AuthHub.Domain.Results;
using AuthHub.Persistence.Abstractions;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Api.UnitTest.Systems.Services.Users;

public class UserServiceTest
{
    [Theory, ClassData(typeof(UserListValidTestData))]
    public async Task GetAllAsync_Should_ReturnUserList_WhenUsersExist(IReadOnlyList<User> usersExpected)
    {
        // Arrange
        Mock<IUserRepository> mockUserRepository = new();

        mockUserRepository
            .Setup(repository => repository.GetAllAsync())
            .ReturnsAsync(usersExpected)
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
            .HaveSameCount(usersExpected);
    }

    [Theory, ClassData(typeof(CreateUserValidTestData))]
    public async Task CreateAsync_Should_ReturnId_WhenUserDoesNotExist(
        CreateUserRequest request)
    {
        // Arrange
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

        mockPasswordHasher.Setup(provider =>
                provider.HashPassword(It.IsAny<User>(), request.Password))
            .Returns(It.IsAny<string>())
            .Verifiable(Times.Once());

        mockUserRepository
            .Setup(repo => repo.IsUniqueByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(true)
            .Verifiable(Times.Once());
        mockUserRepository
            .Setup(repo => repo.IsUniqueByUserNameAsync(It.IsAny<string>()))
            .ReturnsAsync(true)
            .Verifiable(Times.Once());

        mockUserRepository
            .Setup(repo => repo.InsertAsync(It.IsAny<User>()))
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

    [Theory, ClassData(typeof(CreateUserValidTestData))]
    public async Task CreateAsync_Should_ReturnErrorFailure_WhenUserExists(
        CreateUserRequest request)
    {
        // Arrange
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
            .Setup(repo => repo.IsUniqueByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(false)
            .Verifiable(Times.Once());

        mockUserRepository
            .Setup(repo => repo.InsertAsync(It.IsAny<User>()))
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

    [Theory, ClassData(typeof(UserGuidValidTestData))]
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

    [Theory, ClassData(typeof(UserGuidInvalidTestData))]
    public async Task GetUserById_Should_ReturnErrorFailure_WhenUserDoesNotExist(Guid id, User userExpected)
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
        result.IsFailure
            .Should()
            .BeTrue();
        result.Error.Type
            .Should()
            .Be(ErrorType.NotFound);
    }

    [Fact]
    public async Task Update_Should_ReturnUserId_WhenUserExists()
    {
        // Arrange
        Guid id = Guid.NewGuid();
        User userRecord = new()
        {
            Id = id,
            Username = "username",
            FirstName = "Example",
            LastName = "Example",
            Email = "example@email.com",
            Password = "password",
            IsActive = true,
            RoleId = 1
        };
        CreateUserRequest request = new(
            "Name changed",
            "Last name changed",
            "Username changed",
            "example_changed@email.com",
            "Pas12Changed",
            RoleId: 4);

        Mock<IUserRepository> mockUserRepository = new();
        Mock<IUnitOfWork> mockUnitOfWork = new();
        PasswordHasher<User> passwordHasher = new();
        mockUserRepository.Setup(repo => repo.GetByIdAsync(id))
            .ReturnsAsync(userRecord)
            .Verifiable(Times.Once());

        mockUserRepository.Setup(repo =>
                repo.IsUniqueByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(true)
            .Verifiable(Times.Once());

        mockUserRepository.Setup(repo => repo.IsUniqueByUserNameAsync(It.IsAny<string>()))
            .ReturnsAsync(true)
            .Verifiable(Times.Once());

        mockUnitOfWork
            .Setup(unitOfWork => unitOfWork.SaveChangesAsync(default))
            .ReturnsAsync(1)
            .Verifiable(Times.Once()); // A row affected

        UserService userService = new(mockUserRepository.Object, mockUnitOfWork.Object, passwordHasher);

        // Act
        var result = await userService.Update(id, request);

        // Assert
        mockUserRepository.Verify();
        mockUnitOfWork.Verify();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(id);

    }
}
