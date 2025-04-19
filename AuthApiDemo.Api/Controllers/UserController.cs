using AuthApiDemo.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AuthApiDemo.Services.Interfaces;
using AuthApiDemo.ViewModels;

namespace AuthApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public UserController(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel viewModel)
        {
            var model = viewModel.Map();
            
            var success = await _authService.RegisterAsync(model);
            
            if (!success)
                return BadRequest("User already exists");

            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _authService.AuthenticateAsync(model.Username, model.Password);
            if (user == null)
                return Unauthorized("Invalid credentials");

            var token = _authService.GenerateJwtToken(user);
            return Ok(new { Token = token });
        }

        [Authorize]
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
}
