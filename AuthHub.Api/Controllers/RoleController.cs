using AuthHub.Api.Dtos;
using AuthHub.Api.Services.Roles;
using AuthHub.Infrastructure.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthHub.Api.Controllers;

/// <summary>
/// API controller responsible for managing roles.
/// </summary>
/// <param name="roleService">The service responsible for handling business logic related to roles</param>
[Route("api/[controller]")]
//[CustomAuthorize(Permissions.CanViewUsers | Permissions.CanManageRoles)]
public class RoleController(IRoleService roleService) : ApiBaseController
{
    [CustomAuthorize(Permissions.CanViewUsers)]
    [HttpGet, ProducesResponseType<List<RoleResponse>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await roleService.GetAllAsync();
        return result.IsSuccess
            ? Ok(result.Value) 
            : HandleFailure(result);
    }

    [CustomAuthorize(Permissions.CanManageUsers)]
    [HttpPost, ProducesResponseType<string>(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateRoleRequest request)
    {
        var result = await roleService.CreateAsync(request);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetAll), result.Value)
            : HandleFailure(result);
    }
}
