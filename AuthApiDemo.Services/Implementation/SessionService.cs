using System.Security.Cryptography;
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
        var refreshToken = GenerateSecureRefreshToken();
        var session = new Session
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            ExpirationDate = DateTime.UtcNow.Add(duration),
            RefreshToken = refreshToken,
            CreatedOn = DateTime.UtcNow,
            ModifiedOn = DateTime.UtcNow
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
    
    private string GenerateSecureRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }
}
