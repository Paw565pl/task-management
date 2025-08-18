namespace TaskManagement.Backend.Core.ExceptionHandlers;

public record ProblemDetailsExceptionReason(int StatusCode, string Message);
