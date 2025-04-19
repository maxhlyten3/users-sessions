using AuthApiDemo.Services.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthApiDemo.Services.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Session> Sessions => Set<Session>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}