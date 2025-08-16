using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Backend.Core.Dto;

public record PageOptionsDto(
    [Range(1, int.MaxValue)] int PageNumber = 1,
    [Range(1, 100)] int PageSize = 20
)
{
    public const int DefaultPageNumber = 1;
    public const int DefaultPageSize = 20;
};
