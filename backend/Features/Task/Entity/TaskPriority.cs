using System.Text.Json.Serialization;

namespace TaskManagement.Backend.Features.Task.Entity;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TaskPriority
{
    Low,
    Medium,
    High,
}
