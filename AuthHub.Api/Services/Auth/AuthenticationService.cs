using AuthHub.Api.Dtos;
using AuthHub.Domain.Entities;
using AuthHub.Domain.Errors;
using AuthHub.Domain.Repositories;
using AuthHub.Domain.Results;
using AuthHub.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity;

namespace AuthHub.Api.Services.Auth;

public class AuthenticationService(
    IUserRepository userRepository,
    ITokenProvider tokenProvider,
    IPasswordHasher<User> passwordHasher) : IAuthenticationService
{
    public async Task<Result<AccessTokenResponse>> AuthenticateAsync(LoginRequest request)
    {
        var user = await userRepository.GetByUserNameAsync(request.UserName);
        if (user is null)
        {
            return Result.Failure<AccessTokenResponse>(AuthError.InvalidCredentials());
        }
        if (!user.IsActive)
        {
            return Result.Failure<AccessTokenResponse>(AuthError.InvalidCredentials());
        }
        PasswordVerificationResult passwordVerification = passwordHasher
            .VerifyHashedPassword(user, user.Password, request.Password);
        
        if (passwordVerification is PasswordVerificationResult.Failed)
        {
            return Result.Failure<AccessTokenResponse>(AuthError.InvalidCredentials());
        }
        AccessTokenResponse tokenResponse = tokenProvider.GetAccesToken(user);
        return tokenResponse;
    }
}
