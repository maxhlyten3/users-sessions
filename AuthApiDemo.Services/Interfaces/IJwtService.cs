using AuthApiDemo.Services.Data.Models;

namespace AuthApiDemo.Services.Interfaces;

public interface IJwtService
{
    string GenerateJwtToken(Guid sessionId);
}