using Api.UnitTest.Systems.Services.Auth.TestData;
using AuthHub.Api.Dtos;
using AuthHub.Api.Services.Auth;
using AuthHub.Domain.Entities;
using AuthHub.Domain.Repositories;
using AuthHub.Infrastructure.Authorization;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Api.UnitTest.Systems.Services.Auth;

public class AuthServiceTest
{
    [Theory, ClassData(typeof(ValidAccessTokenTestData))]
    public async Task GetAccessToken_Should_ReturnValidAcessToken_WhenLoginDataIsValid(
        LoginRequest input, User expectedUser, AccessTokenResponse expectedToken)
    {
        // Arrange
        Mock<ITokenProvider> mockTokenProvider = new();
        Mock<IUserRepository> mockUserRepository = new();
        Mock<IPasswordHasher<User>> mockPasswordHasher = new();

        mockUserRepository.Setup(repo => repo.GetByUserNameAsync(input.UserName))
            .ReturnsAsync(expectedUser)
            .Verifiable(Times.Once());

        mockPasswordHasher.Setup(provider => 
                provider.VerifyHashedPassword(
                    It.IsAny<User>(), 
                    It.IsAny<string>(), 
                    input.Password))
            .Returns(PasswordVerificationResult.Success)
            .Verifiable(Times.Once());

        mockTokenProvider.Setup(provider => provider.GetAccessToken(It.IsAny<User>()))
            .Returns(expectedToken)
            .Verifiable(Times.Once());

        AuthenticationService service = new(
            mockUserRepository.Object,
            mockTokenProvider.Object,
            mockPasswordHasher.Object);


        // Act
        var result = await service.AuthenticateAsync(input);

        // Assert
        mockUserRepository.Verify();
        mockTokenProvider.Verify();
        mockPasswordHasher.Verify();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeOfType<AccessTokenResponse>();
        result.Value.Should().BeEquivalentTo(expectedToken);
    }
}
