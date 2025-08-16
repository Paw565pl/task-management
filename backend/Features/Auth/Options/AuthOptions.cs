using System.ComponentModel.DataAnnotations;
using TaskManagement.Backend.Core.Validators;

namespace TaskManagement.Backend.Features.Auth.Options;

public class AuthOptions
{
    public const string SectionName = "Auth";

    [NotBlank][Url] public string Authority { get; set; } = string.Empty;

    [NotBlank] public string Audience { get; set; } = string.Empty;
}
