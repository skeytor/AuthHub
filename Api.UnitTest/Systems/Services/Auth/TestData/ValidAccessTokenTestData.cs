using AuthHub.Api.Dtos;
using AuthHub.Domain.Entities;
using Microsoft.AspNetCore.Authentication.BearerToken;

namespace Api.UnitTest.Systems.Services.Auth.TestData;

internal class ValidAccessTokenTestData : TheoryData<LoginRequest, User, AccessTokenResponse>
{
    public ValidAccessTokenTestData()
    {
        Add(
            new LoginRequest("user_name1", "HashedPassword1"),
            new User
            {
                FirstName = "Test1",
                LastName = "Test1",
                Email = "test1@email.com",
                Username = "user_name1",
                Password = "HashedPassword1",
                Id = Guid.NewGuid(),
                IsActive = true,
                RoleId = 1
            },
            new AccessTokenResponse
            {
                AccessToken = "AccessToken1",
                ExpiresIn = 3600,
                RefreshToken = "RefreshToken1"
            });
        Add(
            new LoginRequest("user_name2", "HashedPassword2"),
            new User
            {
                FirstName = "Test2",
                LastName = "Test2",
                Email = "test2@email.com",
                Username = "user_name2",
                Password = "HashedPassword2",
                Id = Guid.NewGuid(),
                IsActive = true,
                RoleId = 2
            },
            new AccessTokenResponse
            {
                AccessToken = "AccessToken2",
                ExpiresIn = 2600,
                RefreshToken = "RefreshToken2"
            });
    }
}
