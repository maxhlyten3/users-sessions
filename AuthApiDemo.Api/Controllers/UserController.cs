using AuthApiDemo.Mappers;
using Microsoft.AspNetCore.Mvc;
using AuthApiDemo.Services.Interfaces;
using AuthApiDemo.ViewModels;

namespace AuthApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IJwtService _jwtService;
        private readonly ISessionService _sessionService;
        private readonly int _expires;
        private readonly IUserService _userService;

        public UserController(IAuthService authService, IJwtService jwtService, ISessionService sessionService, IUserService userService, IConfiguration config)
        {
            _authService = authService;
            _jwtService = jwtService;
            _sessionService = sessionService;
            _userService = userService;
            _expires = int.Parse(config["Session:ExpiresTime:Minutes"]);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
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
                return BadRequest();
            }

            var sessionID = await _sessionService.CreateSessionAsync(user.Id, TimeSpan.FromMinutes(_expires));
            
            var token = _jwtService.GenerateJwtToken(sessionID);
            
            return Ok(new { Token = token });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenModel model)
        {
            var session = await _sessionService.GetSessionByIdAsync(model.SessionId);

            if (session == null || session.ExpirationDate < DateTime.UtcNow)
                return Unauthorized("Session is invalid or expired");

            if (session.RefreshToken != model.RefreshToken)
                return Unauthorized("Refresh token does not match");

            var user = await _userService.GetUserById(session.UserId);
            if (user == null)
                return Unauthorized("User not found");

            var newJwt = _jwtService.GenerateJwtToken(session.Id);

            return Ok(new
            {
                Token = newJwt,
            });
        }



    }
}
