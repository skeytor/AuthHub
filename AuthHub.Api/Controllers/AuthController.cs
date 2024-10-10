using AuthHub.Api.Dtos;
using AuthHub.Api.Extensions;
using AuthHub.Api.Services.Auth;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthHub.Api.Controllers;

/// <summary>
/// Represents a controller for managing authentication of users.
/// </summary>
/// <param name="authService"></param>
[Route("api/[controller]")]
public class AuthController(
    IAuthenticationService authService) : ApiBaseController
{
    [AllowAnonymous]
    [HttpPost("login"), ProducesResponseType<AccessTokenResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await authService.AuthenticateAsync(request);
        return result.IsSuccess
            ? Ok(result.Value) 
            : HandleFailure(result);
    }

    [Authorize]
    [HttpPost("logout")]
    public Task<IActionResult> Logout()
    {
        throw new NotImplementedException();
    }
}
