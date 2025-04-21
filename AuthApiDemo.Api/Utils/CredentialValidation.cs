using System.Text.RegularExpressions;

namespace AuthApiDemo.Utils;

public class CredentialValidation
{
    public bool IsValidPassword(string password)
    {
        var regex = new Regex(@"^(?=.*[A-Z])(?=.*\d).{8,}$");
        return regex.IsMatch(password);
    }
}