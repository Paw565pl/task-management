using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace TaskManagement.Backend.Core.Dto;

public class ValidationProblemDetailsDto : ProblemDetails
{
    public required ICollection<ValidationError> Errors { get; init; }

    public ValidationProblemDetailsDto()
    {
        Status = StatusCodes.Status400BadRequest;
        Title = ReasonPhrases.GetReasonPhrase(StatusCodes.Status400BadRequest);
        Detail = "One or more validation errors occured.";
    }
}

public record ValidationError(string PropertyName, string Message);
