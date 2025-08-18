using Microsoft.AspNetCore.Mvc;
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
                    return new BadRequestObjectResult(
                        new ProblemDetails
                        {
                            Status = statusCode,
                            Title = ReasonPhrases.GetReasonPhrase(statusCode),
                            Detail = "Request body must not be empty.",
                        }
                    );
                }

                const string detail = "Request body is not valid.";

                var hasInvalidBody = errors.Any(kv =>
                    string.Equals(kv.Key, "$", StringComparison.Ordinal)
                );
                if (hasInvalidBody)
                {
                    return new BadRequestObjectResult(
                        new ProblemDetails
                        {
                            Status = statusCode,
                            Title = ReasonPhrases.GetReasonPhrase(statusCode),
                            Detail = detail,
                        }
                    );
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

                            return new { propertyName, message };
                        });
                    })
                    .ToList();

                return new BadRequestObjectResult(
                    new ProblemDetails
                    {
                        Status = statusCode,
                        Title = ReasonPhrases.GetReasonPhrase(statusCode),
                        Detail = detail,
                        Extensions = { ["errors"] = validationErrors },
                    }
                );
            }
        );

        return builder;
    }
}
