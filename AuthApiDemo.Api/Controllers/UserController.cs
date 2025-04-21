using AuthApiDemo.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AuthApiDemo.Services.Interfaces;
using AuthApiDemo.ViewModels;
using AuthApiDemo.Utils;

namespace AuthApiDemo.Controllers
{
    //TODO: add user profile controller. Returns user information (user email, fist name, last name).  [write custom auth attribute that checks if session is valid]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        private readonly ISessionService _sessionService;
        private readonly int _expires;

        public UserController(IAuthService authService, IUserService userService, IJwtService jwtService, ISessionService sessionService, IConfiguration config)
        {
            _authService = authService;
            _userService = userService;
            _jwtService = jwtService;
            _sessionService = sessionService;
            _expires = int.Parse(config["Jwt:ExpiresTime:Minutes"]);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel viewModel)
        {
            CredentialValidation credentialValidation = new CredentialValidation();
            if (!credentialValidation.IsValidPassword(viewModel.Password))
            {
                return BadRequest("The password must be at least 8 characters long, contain at least one capital letter and one number.");
            }
            
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
            {
                return Unauthorized("Bad request");
            }

            var sessionID = _sessionService.CreateSessionAsync(user.Id, TimeSpan.FromMinutes(_expires));
            
            var token = _jwtService.GenerateJwtToken(await sessionID);
            
            return Ok(new { Token = token });
        }

        //TODO: implement refresh token functionality  
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
}
