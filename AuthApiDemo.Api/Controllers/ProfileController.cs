using AuthApiDemo.Services.Interfaces;
using AuthApiDemo.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthApiDemo.Controllers;

public class ProfileController: ControllerBase
{
    private readonly IUserService _userService;
    
    public ProfileController(IUserService userService)
    {
        _userService = userService;
    }

    [Authorize]
    [ServiceFilter(typeof(SessionValidFilter))]
    [HttpGet("profile")]
    public IActionResult GetProfile()
    {
        var username = User.Identity?.Name;
        if (string.IsNullOrEmpty(username)) return Unauthorized();

        var user = _userService.GetUserByUsername(username);
        if (user == null) return NotFound("User not found");

        return Ok(user);
    }
}