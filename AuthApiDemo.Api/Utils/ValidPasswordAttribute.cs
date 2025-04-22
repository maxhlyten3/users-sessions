namespace AuthApiDemo.Utils;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

public class ValidPasswordAttribute: ValidationAttribute
{
    public override bool IsValid(object value)
    {
        var password = value as string;
        if (string.IsNullOrEmpty(password))
            return false;

        var regex = new Regex(@"^(?=.*[A-Z])(?=.*\d).{8,}$");
        return regex.IsMatch(password);
    }

    public override string FormatErrorMessage(string name)
    {
        return "The password must be at least 8 characters long, contain at least one capital letter and one number.";
    }
}