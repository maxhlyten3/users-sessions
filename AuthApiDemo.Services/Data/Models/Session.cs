namespace AuthApiDemo.Services.Data.Models;

public class Session
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime ExpirationDate { get; set; }
}
