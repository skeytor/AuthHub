﻿using AuthHub.Api.Services.UserService;
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
        return Ok(result);
    }
}
