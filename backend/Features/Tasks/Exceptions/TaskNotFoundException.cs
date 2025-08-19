using TaskManagement.Backend.Core.Exceptions;

namespace TaskManagement.Backend.Features.Tasks.Exceptions;

public sealed class TaskNotFoundException()
    : ProblemDetailsException(StatusCodes.Status404NotFound, "Task with given id does not exist.");
