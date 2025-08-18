using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.WebUtilities;

namespace TaskManagement.Backend.Core.Extensions;

public static class ModelStateProblemDetailsExtensions
{
    public static IMvcBuilder MapBindingErrorsToProblemDetails(this IMvcBuilder builder)
    {
        builder.AddJsonOptions(options => options.AllowInputFormatterExceptionMessages = false);
        builder.ConfigureApiBehaviorOptions(options =>
            options.InvalidModelStateResponseFactory = context =>
            {
                const int statusCode = StatusCodes.Status400BadRequest;
                var title = ReasonPhrases.GetReasonPhrase(statusCode);

                var problemDetailsFactory =
                    context.HttpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();

                var modelState = context.ModelState;
                var errors = modelState
                    .Where(kv =>
                        kv.Value?.Errors.Count > 0
                        && !kv.Key.Contains("dto", StringComparison.OrdinalIgnoreCase)
                    )
                    .ToList();

                var hasEmptyBody = errors.Any(kv => string.IsNullOrWhiteSpace(kv.Key));
                if (hasEmptyBody)
                {
                    var problemDetails = problemDetailsFactory.CreateProblemDetails(
                        context.HttpContext,
                        statusCode,
                        title,
                        null,
                        "Request body must not be empty."
                    );
                    return new BadRequestObjectResult(problemDetails);
                }

                var hasInvalidBody = errors.Any(kv =>
                    string.Equals(kv.Key, "$", StringComparison.OrdinalIgnoreCase)
                );
                if (hasInvalidBody)
                {
                    var problemDetails = problemDetailsFactory.CreateProblemDetails(
                        context.HttpContext,
                        statusCode,
                        title,
                        null,
                        "Request body is not valid."
                    );
                    return new BadRequestObjectResult(problemDetails);
                }

                var validationErrors = errors
                    .SelectMany(kv =>
                    {
                        var rawKey = kv.Key;
                        var propertyName = rawKey.Split('.').LastOrDefault() ?? rawKey;

                        return kv.Value!.Errors.Select(err =>
                        {
                            var message = !string.IsNullOrWhiteSpace(err.ErrorMessage)
                                ? err.ErrorMessage
                                : "Invalid value.";

                            return new ValidationError(propertyName, message);
                        });
                    })
                    .ToList();

                var validationProblemDetails = problemDetailsFactory.CreateProblemDetails(
                    context.HttpContext,
                    statusCode,
                    title,
                    null,
                    "One or more validation errors occured."
                );
                validationProblemDetails.Extensions.Add("errors", validationErrors);

                return new BadRequestObjectResult(validationProblemDetails);
            }
        );

        return builder;
    }
}

internal sealed record ValidationError(string PropertyName, string Message);
