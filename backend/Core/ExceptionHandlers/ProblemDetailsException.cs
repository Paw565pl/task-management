namespace TaskManagement.Backend.Core.ExceptionHandlers;

public class ProblemDetailsException(ProblemDetailsExceptionReason problemDetailsExceptionReason)
    : Exception
{
    public int StatusCode { get; } = problemDetailsExceptionReason.StatusCode;
    public override string Message { get; } = problemDetailsExceptionReason.Message;
}
