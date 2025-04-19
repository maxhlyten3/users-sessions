namespace AuthApiDemo.Services.Implementation;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using AuthApiDemo.Services.Data;
using AuthApiDemo.Services.Models;

public class AuthService : IAuthService
{
    private readonly AppDbContext _db;
    private readonly string _jwtKey;

    public AuthService(AppDbContext db, IConfiguration config)
    {
        _db = db;
        _jwtKey = config["Jwt:Key"];
    }

    public async Task<User?> AuthenticateAsync(string username, string password)
    {
        var user = _db.Users.FirstOrDefault(u => u.Username == username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            return null;

        return user;
    }

    public async Task<bool> RegisterAsync(RegisterModel model)
    {
        var existingUser = _db.Users.FirstOrDefault(u => u.Username == model.Username);
        if (existingUser != null) return false;

        var newUser = new User
        {
            Username = model.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password)
        };

        _db.Users.Add(newUser);
        await _db.SaveChangesAsync();
        return true;
    }

    public string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "your_issuer",
            audience: "your_audience",
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
