using AuthHub.Api.Dtos;
using AuthHub.Api.Extensions;
using AuthHub.Api.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthHub.Api.Controllers;

/// <summary>
/// Represents a controller for managing users.
/// </summary>
/// <param name="userService"></param>
[Route("api/[controller]")]
public sealed class UserController(
    IUserService userService) : ApiBaseController
{
    [HttpGet("me")]
    [ProducesResponseType<UserResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Me()
    {
        string userIdClaimValue = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        if (Guid.TryParse(userIdClaimValue, out var userId))
        {
            var result = await userService.GetByIdAsync(userId);
            return result.IsSuccess
                ? Ok(result.Value)
                : NotFound(result.ToProblemDetails());
        }
        return BadRequest();
    }

    [HttpGet]
    [ProducesResponseType<List<UserResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAll()
    {
        var result = await userService.GetAllAsync();
        return result.IsSuccess
            ? Ok(result.Value)
            : BadRequest(result.ToProblemDetails());
    }

    [HttpPost, ProducesResponseType<Guid>(StatusCodes.Status201Created)]
    public async Task<IActionResult> Register([FromBody] CreateUserRequest request)
    {
        var result = await userService.RegisterAsync(request);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetAll), result.Value)
            : BadRequest(result.ToProblemDetails());
    }

    [HttpGet("{id}"), ProducesResponseType<UserResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var result = await userService.GetByIdAsync(id);
        return result.IsSuccess
            ? Ok(result.Value)
            : HandleFailure(result);
    }


    [HttpGet("user/{id}"), ProducesResponseType<UserResponse>(StatusCodes.Status200OK)]
    public async Task<IResult> GetByIds([FromRoute] Guid id)
    {
        var result = await userService.GetByIdAsync(id);
        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.ToProblems();
    }


    [HttpPut("{id}"), ProducesResponseType<Guid>(StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id, CreateUserRequest request)
    {
        var result = await userService.Update(id, request);
        return result.IsSuccess
            ? Ok(result.Value)
            : BadRequest();
    }
}
