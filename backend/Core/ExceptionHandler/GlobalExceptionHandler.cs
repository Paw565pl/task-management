using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace TaskManagement.Backend.Core.ExceptionHandler;

public class GlobalExceptionHandler(
    IProblemDetailsService problemDetailsService,
    ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        ProblemDetails problemDetails;

        if (exception is ProblemDetailsException problemDetailsException)
            problemDetails = new ProblemDetails
            {
                Status = problemDetailsException.StatusCode,
                Title = ReasonPhrases.GetReasonPhrase(problemDetailsException.StatusCode),
                Detail = problemDetailsException.Message,
            };
        else
        {
            logger.LogError("unexpected error has occurred: {}", exception.StackTrace);

            problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = ReasonPhrases.GetReasonPhrase(StatusCodes.Status500InternalServerError),
                Detail = "Unexpected error has occured.",
            };
        }

        httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
        var problemDetailsContext = new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problemDetails
        };

        return await problemDetailsService.TryWriteAsync(problemDetailsContext);
    }
}
