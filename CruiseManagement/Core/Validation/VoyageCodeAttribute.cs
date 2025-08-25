using Core.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Core.Validation;

public class VoyageCodeAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var voyage = validationContext.ObjectInstance as BaseVoyage;
        var code = voyage?.Code;

        if (string.IsNullOrEmpty(code))
            return new ValidationResult("Voyage code is required");

        // Format: tri slova "-" 3 broja "-" 3 broja
        var pattern = @"^[A-Z]{3}-\d{3}-\d{3}$";

        if (!Regex.IsMatch(code, pattern))
        {
            return new ValidationResult("Voyage code must be in format: ABC-123-456 (3 letters, hyphen, 3 numbers, hyphen, 3 numbers)");
        }

        return ValidationResult.Success;
    }
}
