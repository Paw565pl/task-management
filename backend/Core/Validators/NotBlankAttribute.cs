using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Backend.Core.Validators;

[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property)]
public class NotBlankAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        // allow optional fields
        if (value is null)
            return ValidationResult.Success;

        if (string.IsNullOrWhiteSpace((string)value))
            return new ValidationResult($"{validationContext.DisplayName} cannot be empty.");

        return ValidationResult.Success;
    }
}
