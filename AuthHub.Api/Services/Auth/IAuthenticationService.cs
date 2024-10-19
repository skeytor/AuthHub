using AuthHub.Api.Dtos;
using AuthHub.Domain.Results;
using Microsoft.AspNetCore.Authentication.BearerToken;

namespace AuthHub.Api.Services.Auth;

public interface IAuthenticationService
{
    Task<Result<AccessTokenResponse>> AuthenticateAsync(LoginRequest request);
}
