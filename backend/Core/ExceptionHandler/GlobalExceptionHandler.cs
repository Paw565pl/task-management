using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace TaskManagement.Backend.Core.ExceptionHandler;

public partial class GlobalExceptionHandler(
    IProblemDetailsService problemDetailsService,
    ILogger<GlobalExceptionHandler> logger
) : IExceptionHandler
{
    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Unhandled exception while processing {method} {path} - {traceIdentifier}"
    )]
    private static partial void LogUnhandledException(
        ILogger logger,
        Exception exception,
        string method,
        string path,
        string traceIdentifier
    );

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        var problemDetails = new ProblemDetails();

        if (exception is ProblemDetailsException problemDetailsException)
        {
            problemDetails.Status = problemDetailsException.StatusCode;
            problemDetails.Title = ReasonPhrases.GetReasonPhrase(
                problemDetailsException.StatusCode
            );
            problemDetails.Detail = problemDetailsException.Message;
        }
        else
        {
            LogUnhandledException(
                logger,
                exception,
                httpContext.Request.Method,
                httpContext.Request.Path,
                httpContext.TraceIdentifier
            );

            problemDetails.Status = StatusCodes.Status500InternalServerError;
            problemDetails.Title = ReasonPhrases.GetReasonPhrase(
                StatusCodes.Status500InternalServerError
            );
            problemDetails.Detail = "Unexpected error has occured.";
        }

        httpContext.Response.StatusCode =
            problemDetails.Status ?? StatusCodes.Status500InternalServerError;
        var problemDetailsContext = new ProblemDetailsContext
        {
            ProblemDetails = problemDetails,
            HttpContext = httpContext,
            Exception = exception,
        };

        return await problemDetailsService.TryWriteAsync(problemDetailsContext);
    }
}
