using AuthHub.Api.Dtos;
using AuthHub.Api.Services.UserService;
using Microsoft.AspNetCore.Mvc;

namespace AuthHub.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class UserController(IUserService userService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await userService.GetAllUsers();
        return result.IsSuccess
            ? Ok(result.Value)
            : BadRequest();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var result = await userService.Create(request);
        return result.IsSuccess
            ? CreatedAtAction(nameof(Create), result.Value)
            : BadRequest();
    }
}
