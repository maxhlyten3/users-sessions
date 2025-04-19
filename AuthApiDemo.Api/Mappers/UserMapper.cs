using AuthApiDemo.Services.Data.Models;
using AuthApiDemo.ViewModels;

namespace AuthApiDemo.Mappers;

public static class UserMapper
{
    //without 'this' usage example: UserMapper.Map(user) 
    //with 'this' usage example: user.Map() 
    
    public static UserViewModel Map(this User user)
    {
        return new UserViewModel
        {
            Id = user.Id,
            Username = user.Username,
            PasswordHash = user.PasswordHash,
        };
    }
}