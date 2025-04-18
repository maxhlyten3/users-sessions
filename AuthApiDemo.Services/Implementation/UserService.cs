using AuthApiDemo.Services.Data.Models;
using AuthApiDemo.Services.Interfaces;

namespace AuthApiDemo.Services.Implementation;

using Data;
using Models;

public class UserService : IUserService
{
    private readonly AppDbContext _db;

    public UserService(AppDbContext db)
    {
        _db = db;
    }

    public User? GetUserByUsername(string username)
    {
        return _db.Users.FirstOrDefault(u => u.Username == username);
    }
}
