namespace TaskManagement.Backend.Core.Dto;

public record PageOptionsDto(
    int PageNumber = 1,
    int PageSize = 20
);
