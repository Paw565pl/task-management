using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Backend.Core.Validators;

public class NotBlankAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            return new ValidationResult($"{validationContext.DisplayName} cannot be empty.");

        return ValidationResult.Success;
    }
}
