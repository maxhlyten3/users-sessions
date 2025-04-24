using System.Text.RegularExpressions;
using AuthApiDemo.Services.Data.Models;
using AuthApiDemo.Services.Interfaces;

namespace AuthApiDemo.Services.Implementation;

using Data;
using Models;

public class AuthService : IAuthService
{
    private readonly AppDbContext _db;

    public AuthService(AppDbContext db)
    {
        _db = db;
    }
    
    public async Task<User?> AuthenticateAsync(string username, string password)
    {
        var user = _db.Users.FirstOrDefault(u => u.Username == username);
        if (user == null || string.IsNullOrEmpty(user.PasswordHash))
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
    
}
