using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace TaskManagement.Backend.Core.Dtos;

public class ValidationFailureResponseDto : ProblemDetails
{
    public ICollection<ValidationError> Errors { get; private init; } = [];

    private ValidationFailureResponseDto() { }

    public static ValidationFailureResponseDto FromProblemDetails(
        ProblemDetails problemDetails,
        ICollection<ValidationError> errors
    )
    {
        var extensions = new Dictionary<string, object?>(problemDetails.Extensions);

        return new ValidationFailureResponseDto
        {
            Type = problemDetails.Type,
            Title = ReasonPhrases.GetReasonPhrase(StatusCodes.Status400BadRequest),
            Status = StatusCodes.Status400BadRequest,
            Detail = "One or more validation errors occured.",
            Instance = problemDetails.Instance,
            Extensions = extensions,
            Errors = errors,
        };
    }
}

public record ValidationError(string PropertyName, string Message);
