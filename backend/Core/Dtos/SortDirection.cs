using System.Text.Json.Serialization;

namespace TaskManagement.Backend.Core.Dtos;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SortDirection
{
    Asc,
    Desc,
}
