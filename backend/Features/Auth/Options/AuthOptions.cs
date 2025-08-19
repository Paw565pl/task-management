using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Backend.Features.Auth.Options;

public class AuthOptions
{
    public const string SectionName = "Auth";

    [Url]
    [Required(AllowEmptyStrings = false)]
    public string Authority { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false)]
    public string Audience { get; set; } = string.Empty;
}
