using AuthApiDemo.Services.Data.Models;

namespace AuthApiDemo.Services.Interfaces;

public interface IUserService
{
    User? GetUserByUsername(string username);
    Task<User?> GetUserById(Guid userId);
}
