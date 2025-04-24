namespace AuthApiDemo.ViewModels;

public class UserViewModel
{
    public Guid Id { get; set; }
    public string Username { get; set; } = "";
    public string PasswordHash { get; set; } = "";
}