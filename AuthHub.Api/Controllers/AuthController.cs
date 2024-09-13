using AuthHub.Api.Dtos;
using AuthHub.Api.Services.Auth;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Mvc;

namespace AuthHub.Api.Controllers;

/// <summary>
/// Represents a controller for managing authentication of users.
/// </summary>
/// <param name="authService"></param>
[Route("api/[controller]")]
[ApiController]
public class AuthController(
    IAuthenticationService authService) : ControllerBase
{
    [HttpPost("login"), ProducesResponseType<AccessTokenResponse>(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await authService.AuthenticateAsync(request);
        return result.IsSuccess
            ? Ok(result.Value) 
            : BadRequest();
    }
}
