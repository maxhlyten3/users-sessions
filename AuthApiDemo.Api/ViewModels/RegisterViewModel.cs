using System.ComponentModel.DataAnnotations;
using AuthApiDemo.Utils;

namespace AuthApiDemo.ViewModels;

public class RegisterViewModel
{
    [Required]
    public string Username { get; set; }
    
    [Required]
    [ValidPassword]
    public string Password { get; set; }
}