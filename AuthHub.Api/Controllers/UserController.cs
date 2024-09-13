using AuthHub.Api.Dtos;
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
[ApiController]
public sealed class UserController(
    IUserService userService) : ControllerBase
{
    [HttpGet("me"), ProducesResponseType<UserResponse>(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Me()
    {
        //var claimValue = claims.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        string claimValue = "";
        if (Guid.TryParse(claimValue, out var userId)) 
        { 
            var result = await userService.GetByIdAsync(userId);
            return result.IsSuccess
                ? Ok(result.Value) 
                : NotFound();
        }
        return BadRequest();
    }

    [HttpGet, ProducesResponseType<List<UserResponse>>(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAll()
    {
        var result = await userService.GetAllAsync();
        return result.IsSuccess
            ? Ok(result.Value)
            : BadRequest();
    }

    [HttpPost, ProducesResponseType<Guid>(StatusCodes.Status201Created), ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
    {
        var result = await userService.CreateAsync(request);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetAll), result.Value)
            : BadRequest();
    }

    [HttpGet("{id}"), ProducesResponseType<UserResponse>(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var result = await userService.GetByIdAsync(id);
        return result.IsSuccess
            ? Ok(result.Value)
            : NotFound();
    }

    [HttpPut("{id}"), ProducesResponseType<Guid>(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id, CreateUserRequest request)
    {
        var result = await userService.Update(id, request);
        return result.IsSuccess
            ? Ok(result.Value)
            : BadRequest();
    }
}
