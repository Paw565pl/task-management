using System.Text.Json.Serialization;

namespace TaskManagement.Backend.Features.Task.Entity;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TaskStatus
{
    Todo,
    InProgress,
    OnHold,
    Done,
}
