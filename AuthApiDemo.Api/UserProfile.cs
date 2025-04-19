using AutoMapper;
using AuthApiDemo.Services.Models;
using AuthApiDemo.Models;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<AuthApiDemo.Services.Models.User, AuthApiDemo.Models.User>();
        CreateMap<AuthApiDemo.Models.User, AuthApiDemo.Services.Models.User>();
    }
}