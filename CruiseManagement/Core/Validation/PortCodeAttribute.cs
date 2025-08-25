using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Core.Validation;

public class PortCodeAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
            return ValidationResult.Success;

        var code = value.ToString();

        // Format: 3 slova
        var pattern = @"^[A-Z]{3}$";

        if (!Regex.IsMatch(code, pattern))
        {
            return new ValidationResult("Port code must be exactly 3 uppercase letters");
        }

        return ValidationResult.Success;
    }
}
