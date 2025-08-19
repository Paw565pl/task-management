using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Backend.Core.Validators;

[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property)]
public class NotBlankAttribute() : ValidationAttribute("{0} cannot be empty.")
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        // allow optional fields and not string fields
        if (value is not string stringValue)
            return ValidationResult.Success;

        if (string.IsNullOrWhiteSpace(stringValue))
            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));

        return ValidationResult.Success;
    }
}
