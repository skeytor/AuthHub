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
[CustomAuthorize(Permissions.CanViewRoles | Permissions.CanManageRoles)]
public class RoleController(IRoleService roleService) : ApiBaseController
{
    [HttpGet, ProducesResponseType<List<RoleResponse>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await roleService.GetAllAsync();
        return result.IsSuccess
            ? Ok(result.Value) 
            : HandleFailure(result);
    }

    [HttpPost, ProducesResponseType<string>(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateRoleRequest request)
    {
        var result = await roleService.CreateAsync(request);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetAll), result.Value)
            : HandleFailure(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] CreateRoleRequest request)
    {
        var result = await roleService.UpdateAsync(id, request);
        return result.IsSuccess
            ? Ok(result.Value)
            : HandleFailure(result);
    }
}
