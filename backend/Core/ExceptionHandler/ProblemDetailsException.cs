namespace TaskManagement.Backend.Core.ExceptionHandler;

public class ProblemDetailsException(ProblemDetailsExceptionReason problemDetailsExceptionReason) : Exception
{
    public int StatusCode { get; } = problemDetailsExceptionReason.StatusCode;
    public override string Message { get; } = problemDetailsExceptionReason.Message;
}
