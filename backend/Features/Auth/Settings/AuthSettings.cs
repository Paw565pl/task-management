using System.ComponentModel.DataAnnotations;
using TaskManagement.Backend.Core.Validators;

namespace TaskManagement.Backend.Features.Auth.Settings;

public class AuthSettings
{
    public const string Section = "Auth";

    [NotBlank, Url] public string Authority { get; set; } = string.Empty;

    [NotBlank] public string Audience { get; set; } = string.Empty;
}
