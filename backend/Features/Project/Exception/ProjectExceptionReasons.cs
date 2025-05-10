using TaskManagement.Backend.Core.ExceptionHandler;

namespace TaskManagement.Backend.Features.Project.Exception;

public static class ProjectExceptionReasons
{
    public static ProblemDetailsExceptionReason NotFound =>
        new(StatusCodes.Status404NotFound, "Project with given id does not exist.");

    public static ProblemDetailsExceptionReason NameNotUnique =>
        new(StatusCodes.Status409Conflict, "Name must be unique.");
}
