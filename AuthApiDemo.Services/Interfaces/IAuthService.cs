using AuthApiDemo.Services.Data.Models;
using AuthApiDemo.Services.Models;

namespace AuthApiDemo.Services.Interfaces;

public interface IAuthService
{
    Task<User?> AuthenticateAsync(string username, string password);
    Task<bool> RegisterAsync(RegisterModel model);
}