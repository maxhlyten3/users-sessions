using AuthApiDemo.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AuthApiDemo.Services.Interfaces;
using AuthApiDemo.ViewModels;

namespace AuthApiDemo.Controllers
{
    //TODO: add user profile controller. Returns user information (user email, fist name, last name).  [write custom auth attribute that checks if session is valid]
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
            //add password validation (at least 1 number, at least 1 Uppercase symbol, min length 8)
            var model = viewModel.Map();
            
            var success = await _authService.RegisterAsync(model);
            
            if (!success)
                return BadRequest("User already exists");

            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            //TODO: here we check only if user has login/password
            var user = await _authService.AuthenticateAsync(model.Username, model.Password);
            if (user == null)
            {
                //return bad request
                return Unauthorized("Invalid credentials");
            }
            
            //iniciate a new session (duration in minutes)
            //var sessionID = sessionService.CreateSession(userId)
            //var token = _authService.GenerateJwtToken(sessionID);
            
            //move this code from auth service to (new) JwtToken
            var token = _authService.GenerateJwtToken(user);
            
            return Ok(new { Token = token });
        }

        //TODO: implement refresh token functionality  
        
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
