using AuthHub.Domain.Entities;
using Microsoft.AspNetCore.Authentication.BearerToken;

namespace AuthHub.Infrastructure.Authorization;

public interface ITokenProvider
{
    AccessTokenResponse GetAccessToken(User user);
}
