using System.Text.Json.Serialization;

namespace TaskManagement.Backend.Core.Dto;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SortDirection
{
    Asc,
    Desc
}
