using TaskManagement.Backend.Core.Exceptions;

namespace TaskManagement.Backend.Features.Projects.Exceptions;

public sealed class ProjectNotFoundException()
    : ProblemDetailsException(
        StatusCodes.Status404NotFound,
        "Project with given id does not exist."
    );
