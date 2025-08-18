using System.Text.Json.Serialization;

namespace TaskManagement.Backend.Features.Tasks.Entities;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TaskPriority
{
    Low,
    Medium,
    High,
}
