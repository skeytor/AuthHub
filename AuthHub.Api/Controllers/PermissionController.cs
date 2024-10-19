using AuthHub.Api.Services.Permissions;
using Microsoft.AspNetCore.Mvc;

namespace AuthHub.Api.Controllers;

[Route("api/[controller]")]
public class PermissionController(IPermissionService permissionService) : ApiBaseController
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await permissionService.GetAllAsync();
        return result.IsSuccess 
            ? Ok(result.Value) 
            : HandleFailure(result);
    }
}
