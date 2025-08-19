using TaskManagement.Backend.Core.Exceptions;

namespace TaskManagement.Backend.Features.Projects.Exceptions;

public sealed class ProjectNameNotUniqueException()
    : ProblemDetailsException(StatusCodes.Status409Conflict, "Name must be unique.");
