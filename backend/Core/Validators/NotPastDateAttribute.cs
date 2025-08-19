using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Backend.Core.Validators;

[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property)]
public class NotPastDateAttribute() : ValidationAttribute("{0} must be today or a future date.")
{
    public bool IsTodayAllowed { get; set; } = true;

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        // allow optional fields
        if (value is null)
            return ValidationResult.Success;

        DateOnly date;

        if (value is DateOnly d)
            date = d;
        else if (value is DateTime dt)
            date = DateOnly.FromDateTime(dt);
        // allow if not valid type
        else
            return ValidationResult.Success;

        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        if (date < today || !IsTodayAllowed && date == today)
            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));

        return ValidationResult.Success;
    }
}
