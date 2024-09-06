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
    [Theory, ClassData(typeof(ValidUserListTestData))]
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

    [Theory, ClassData(typeof(ValidCreateUserTestData))]
    public async Task CreateAsync_Should_ReturnId_WhenUserDoesNotExist(
        CreateUserRequest input)
    {
        // Arrange
        User expected = new()
        {
            Id = Guid.NewGuid(),
            FirstName = input.FirstName,
            LastName = input.LastName,
            Username = input.UserName,
            Email = input.Email,
            RoleId = input.RoleId,
            IsActive = true
        };

        Mock<IUserRepository> mockUserRepository = new();
        Mock<IUnitOfWork> mockUnitOfWork = new();
        Mock<IPasswordHasher<User>> mockPasswordHasher = new();

        mockPasswordHasher.Setup(provider =>
                provider.HashPassword(It.IsAny<User>(), input.Password))
            .Returns(It.IsAny<string>())
            .Verifiable(Times.Once());

        mockUserRepository
            .Setup(repo => repo.EmailExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(false)
            .Verifiable(Times.Once());
        mockUserRepository
            .Setup(repo => repo.UserNameExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(false)
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
        var result = await userService.CreateAsync(input);

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

    [Theory, ClassData(typeof(ValidCreateUserTestData))]
    public async Task CreateAsync_Should_ReturnErrorFailure_WhenUserExists(
        CreateUserRequest input)
    {
        // Arrange
        User expected = new()
        {
            Id = Guid.NewGuid(),
            FirstName = input.FirstName,
            LastName = input.LastName,
            Username = input.UserName,
            Email = input.Email,
            Password = input.Password,
            RoleId = input.RoleId,
            IsActive = true
        };
        Mock<IUserRepository> mockUserRepository = new();
        Mock<IUnitOfWork> mockUnitOfWork = new();

        mockUserRepository
            .Setup(repo => repo.EmailExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(true)
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
        var result = await userService.CreateAsync(input);

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

    [Theory, ClassData(typeof(ValidUserTestData))]
    public async Task GetUserById_Should_ReturnUser_WhenUserExists(Guid id, User expected)
    {
        // Arrange
        Mock<IUserRepository> mockUserRepository = new();
        mockUserRepository
            .Setup(repo => repo.GetByIdAsync(id))
            .ReturnsAsync(expected)
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
            .Be(expected.LastName);
    }

    [Theory, ClassData(typeof(InvalidUserTestData))]
    public async Task GetUserById_Should_ReturnErrorFailure_WhenUserDoesNotExist(
        Guid id, User expected)
    {
        // Arrange
        Mock<IUserRepository> mockUserRepository = new();
        mockUserRepository
            .Setup(repo => repo.GetByIdAsync(id))
            .ReturnsAsync(expected)
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

    [Theory, ClassData(typeof(ValidUpdateUserTestData))]
    public async Task Update_Should_ReturnUserId_WhenUserExists(
        CreateUserRequest input, User userToUpdate)
    {
        // Arrange
        Guid userId = userToUpdate.Id;
        User expectedResult = new()
        {
            Id = userId,
            Username = input.UserName,
            FirstName = input.FirstName,
            LastName = input.LastName,
            Email = input.Email,
            Password = input.Password,
            IsActive = true,
            RoleId = input.RoleId,
        };

        Mock<IUserRepository> mockUserRepository = new();
        Mock<IUnitOfWork> mockUnitOfWork = new();
        PasswordHasher<User> passwordHasher = new();
        mockUserRepository.Setup(repo => repo.GetByIdAsync(userId))
            .ReturnsAsync(userToUpdate)
            .Verifiable(Times.Once());

        mockUserRepository.Setup(repo =>
                repo.EmailExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(false)
            .Verifiable(Times.Once());

        mockUserRepository.Setup(repo => repo.UserNameExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(false)
            .Verifiable(Times.Once());

        mockUnitOfWork
            .Setup(unitOfWork => unitOfWork.SaveChangesAsync(default))
            .ReturnsAsync(1)
            .Verifiable(Times.Once()); // A row affected

        UserService userService = new(
            mockUserRepository.Object, 
            mockUnitOfWork.Object, 
            passwordHasher);

        // Act
        var result = await userService.Update(userId, input);

        // Assert
        mockUserRepository.Verify();
        mockUnitOfWork.Verify();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty().And.Be(userId);

    }
}
