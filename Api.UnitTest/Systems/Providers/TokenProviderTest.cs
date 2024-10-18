using AuthHub.Domain.Entities;
using AuthHub.Infrastructure.Authorization;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System.IdentityModel.Tokens.Jwt;

namespace Api.UnitTest.Systems.Providers;

public class TokenProviderTest
{
    [Fact]
    public void GetToken_Should_ReturnValidToken()
    {
        // Arrange
        Mock<IOptions<OptionsToken>> mockOptions = new();
        OptionsToken optionsToken = new()
        {
            SecretKey = ":(a4:B4Z*qp@Lr]-hvwqZ`Y3q(;7o@/7c0MZZ,pdJ<qI^oA:3>`Hytf+g>&~Mn-Fake-Secret-Key",
            Issuer = "some-issuer",
            Audience = "some-auidience"
        };
        mockOptions.Setup(option => option.Value).Returns(optionsToken);
        TokenProvider tokenProvider = new(mockOptions.Object);
        User user = new()
        {
            Id = Guid.NewGuid(),
            FirstName = "First Name",
            LastName = "Last Name",
            Username = "user_name",
            Email = "email@example.com",
            Password = "Pass",
            RoleId = 1,
            Role = new() { Id = 1, Name = "Admin", Description = "Admin user" }
        };
        // Act
        var token = tokenProvider.GetAccessToken(user);

        // Assert
        token.Should().NotBeNull();
        var decodedToken = new JwtSecurityTokenHandler().ReadJwtToken(token.AccessToken);
        decodedToken.Header.Alg.Should().Be(SecurityAlgorithms.HmacSha256);
        decodedToken.Claims.Any().Should().BeTrue();
        decodedToken.Claims.Should().Contain(claim => 
            claim.Type == JwtRegisteredClaimNames.Sub &&
            claim.Value == user.Id.ToString());
    }
}
