namespace AuthApiDemo.Services.Implementation;

using AuthApiDemo.Services.Models;

public interface IAuthService
{
    string GenerateJwtToken(User user);
    Task<User?> AuthenticateAsync(string username, string password);
    Task<bool> RegisterAsync(RegisterModel model);
}