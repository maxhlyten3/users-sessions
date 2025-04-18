using AuthApiDemo.Models;

namespace AuthApiDemo.Services.Implementation;

public interface IAuthService
{
    string GenerateJwtToken(User user);
    Task<User?> AuthenticateAsync(string username, string password);
    Task<bool> RegisterAsync(RegisterModel model);
}