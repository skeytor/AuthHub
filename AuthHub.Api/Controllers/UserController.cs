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
        var result = await userService.GetAllAsync();
        return result.IsSuccess
            ? Ok(result.Value)
            : BadRequest();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
    {
        var result = await userService.CreateAsync(request);
        return result.IsSuccess
            ? CreatedAtAction(nameof(Create), result.Value)
            : BadRequest();
    }
}
