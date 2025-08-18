using System.Text.Json.Serialization;

namespace TaskManagement.Backend.Features.Tasks.Entities;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TaskStatus
{
    Todo,
    InProgress,
    OnHold,
    Done,
}
