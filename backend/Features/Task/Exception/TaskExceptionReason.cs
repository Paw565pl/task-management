using TaskManagement.Backend.Core.ExceptionHandler;

namespace TaskManagement.Backend.Features.Task.Exception;

public static class TaskExceptionReason
{
    public static readonly ProblemDetailsExceptionReason NotFound = new(
        StatusCodes.Status404NotFound,
        "Task with given id does not exist."
    );
}
