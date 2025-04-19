using AuthApiDemo.Services.Models;
using AuthApiDemo.ViewModels;

namespace AuthApiDemo.Mappers;

public static class RegisterModelMapper
{
    //without 'this' usage example: RegisterMapper.Map(Register) 
    //with 'this' usage example: Register.Map() 

    public static RegisterModel Map(this RegisterViewModel Register)
    {
        return new RegisterModel
        {
            Username = Register.Username,
            Password = Register.Password
        };
    }
}