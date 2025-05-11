using TaskManagement.Backend.Core.ExceptionHandler;

namespace TaskManagement.Backend.Features.Task.Exception;

public static class TaskExceptionReason
{
    public static ProblemDetailsExceptionReason NotFound =>
        new(StatusCodes.Status404NotFound, "Task with given id does not exist.");
}
