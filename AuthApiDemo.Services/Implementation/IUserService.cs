namespace AuthApiDemo.Services.Implementation;

using AuthApiDemo.Models;

public interface IUserService
{
    User? GetUserByUsername(string username);
}
