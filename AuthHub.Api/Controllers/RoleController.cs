using AuthHub.Api.Dtos;
using AuthHub.Api.Services.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthHub.Api.Controllers;

/// <summary>
/// API controller responsible for managing roles.
/// </summary>
/// <param name="roleService">The service responsible for handling business logic related to roles</param>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class RoleController(IRoleService roleService) : ControllerBase
{
    /// <summary>
    /// Retrieves all roles in the system
    /// </summary>
    /// <returns>
    /// An <see cref="IActionResult"/> with a list of <see cref="RoleResponse"/>/>
    /// </returns>
    [HttpGet, ProducesResponseType<List<RoleResponse>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await roleService.GetAllAsync();
        return Ok(result.Value);
    }

    /// <summary>
    /// Creates a new role in the system.
    /// </summary>
    /// <param name="request">The data required to create a new role</param>
    /// <returns>
    /// An <see cref="IActionResult"/> indicating whether the role was created successfully.
    /// Returns HTTP status 201 (Created) with the created role's name if it was successfully, or 404 (Bad Request)
    /// if the creation was failure.
    /// </returns>
    [HttpPost, ProducesResponseType<string>(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateRoleRequest request)
    {
        var result = await roleService.CreateAsync(request);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetAll), result.Value)
            : BadRequest();
    }
}
