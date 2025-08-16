namespace TaskManagement.Backend.Core.Dto;

public record PageResponseDto<T>(
    ICollection<T> Content,
    int TotalItems,
    int PageNumber,
    int PageSize
)
{
    public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
    public bool HasNextPage => PageNumber < TotalPages;
    public bool HasPreviousPage => PageNumber > 1 && PageNumber <= TotalPages;
}
