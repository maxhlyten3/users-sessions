using AuthApiDemo.Services.Data.Models;

namespace AuthApiDemo.Services.Interfaces;

public interface ISessionService
{
    Task<Guid> CreateSessionAsync(Guid userId, TimeSpan duration);
    Task<Session> GetSessionByIdAsync(Guid sessionId);
    Task<bool> IsSessionValidAsync(Guid sessionId);
}