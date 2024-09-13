using AuthHub.Domain.Entities;
using Microsoft.AspNetCore.Authentication.BearerToken;

namespace AuthHub.Infrastructure.Authentication;

public interface ITokenProvider
{
    AccessTokenResponse GetAccesToken(User user);
}
