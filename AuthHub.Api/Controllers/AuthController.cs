using AuthHub.Api.Dtos;
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
[ApiController]
[Authorize]
public class AuthController(
    IAuthenticationService authService) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("login"), ProducesResponseType<AccessTokenResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await authService.AuthenticateAsync(request);
        return result.IsSuccess
            ? Ok(result.Value) 
            : BadRequest();
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        throw new NotImplementedException();
    }
}
