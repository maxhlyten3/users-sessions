namespace AuthApiDemo.Services.Implementation;

using AuthApiDemo.Services.Data;
using AuthApiDemo.Services.Models;

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
