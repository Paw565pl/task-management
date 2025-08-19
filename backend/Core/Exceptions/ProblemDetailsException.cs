namespace TaskManagement.Backend.Core.Exceptions;

public class ProblemDetailsException(int statusCode, string message) : Exception(message)
{
    public int StatusCode { get; } = statusCode;
}
