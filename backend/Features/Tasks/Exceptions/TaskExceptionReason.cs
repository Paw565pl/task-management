using TaskManagement.Backend.Core.ExceptionHandlers;

namespace TaskManagement.Backend.Features.Tasks.Exceptions;

public static class TaskExceptionReason
{
    public static readonly ProblemDetailsExceptionReason NotFound = new(
        StatusCodes.Status404NotFound,
        "Task with given id does not exist."
    );
}
