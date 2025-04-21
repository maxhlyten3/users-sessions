using AuthApiDemo.Services.Data;
using AuthApiDemo.Services.Data.Models;
using AuthApiDemo.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuthApiDemo.Services.Implementation;

public class SessionService : ISessionService
{
    private readonly AppDbContext _db;

    public SessionService(AppDbContext dbContext)
    {
        _db = dbContext;
    }

    public async Task<Guid> CreateSessionAsync(Guid userId, TimeSpan duration)
    {
        var session = new Session
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            ExpirationDate = DateTime.UtcNow.Add(duration)
        };
        
        _db.Sessions.Add(session);
        await _db.SaveChangesAsync();

        return session.Id;
    }

    public async Task<Session> GetSessionByIdAsync(Guid sessionId)
    {
        return await _db.Sessions
            .FirstOrDefaultAsync(s => s.Id == sessionId);
    }

    public async Task<bool> IsSessionValidAsync(Guid sessionId)
    {
        var session = await GetSessionByIdAsync(sessionId);
        if (session == null) return false;

        return session.ExpirationDate > DateTime.UtcNow;
    }
}
