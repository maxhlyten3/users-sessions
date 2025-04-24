namespace AuthApiDemo.ViewModels;

public class RefreshTokenModel
{
    public Guid SessionId { get; set; }
    public string RefreshToken { get; set; }
}