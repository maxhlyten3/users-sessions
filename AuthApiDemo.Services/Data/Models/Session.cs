namespace AuthApiDemo.Services.Data.Models;

public class Session
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string RefreshToken { get; set; }
    public DateTime ExpirationDate { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }
}
