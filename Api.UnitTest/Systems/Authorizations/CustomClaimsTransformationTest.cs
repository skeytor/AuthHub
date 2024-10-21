using AuthHub.Domain.Entities;
using AuthHub.Domain.Repositories;
using AuthHub.Infrastructure.Authorization;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Security.Claims;

namespace Api.UnitTest.Systems.Authorizations;

public class CustomClaimsTransformationTest
{
    [Fact]
    public async Task TransformAsync_Should_Return_ClaimPrincipalWithPermissionClaims()
    {
        // Arrange
        Claim[] claims =
        [
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, "user-test")
        ];
        ClaimsPrincipal principal = new(new ClaimsIdentity(claims));

        Mock<IUserRepository> mockUserRepository = new();
        
        mockUserRepository.Setup(repo => repo.GetPermissionByUserIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync([nameof(Permissions.CanViewUsers), nameof(Permissions.CanViewRoles)])
            .Verifiable(Times.Once());

        int userPermissionsValue = (int)(Permissions.CanViewUsers | Permissions.CanViewRoles); // it should be 5

        CustomClaimsTransformation claimsTransformer = new(mockUserRepository.Object);

        // Act
        ClaimsPrincipal transformedPrincipal = await claimsTransformer.TransformAsync(principal);

        // Assert
        transformedPrincipal.Claims
            .Should()
            .Contain(x => x.Type == CustomClaimTypes.Permissions);
        transformedPrincipal.Claims
            .Should()
            .Contain(x => x.Value == userPermissionsValue.ToString());
    }
}
