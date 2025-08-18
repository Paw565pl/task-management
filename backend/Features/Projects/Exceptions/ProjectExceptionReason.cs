using TaskManagement.Backend.Core.ExceptionHandlers;

namespace TaskManagement.Backend.Features.Projects.Exceptions;

public static class ProjectExceptionReason
{
    public static readonly ProblemDetailsExceptionReason NotFound = new(
        StatusCodes.Status404NotFound,
        "Project with given id does not exist."
    );

    public static readonly ProblemDetailsExceptionReason NameNotUnique = new(
        StatusCodes.Status409Conflict,
        "Name must be unique."
    );
}
