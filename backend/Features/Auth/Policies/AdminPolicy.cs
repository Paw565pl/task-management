using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;

namespace TaskManagement.Backend.Features.Auth.Policies;

public static class AdminPolicy
{
    public const string PolicyName = "Admin";

    public static AuthorizationOptions AddAdminPolicy(this AuthorizationOptions options)
    {
        options.AddPolicy(
            PolicyName,
            builder =>
                builder.RequireAssertion(context =>
                {
                    var realmAccessClaim = context.User.FindFirst(claim =>
                        string.Equals(claim.Type, "realm_access", StringComparison.Ordinal)
                    );
                    if (realmAccessClaim is null)
                        return false;

                    var realmAccess = JsonSerializer.Deserialize<RealmAccess>(
                        realmAccessClaim.Value
                    );
                    if (realmAccess is null)
                        return false;

                    return realmAccess.Roles.Contains("admin");
                })
        );

        return options;
    }
}

internal sealed record RealmAccess([property: JsonPropertyName("roles")] HashSet<string> Roles);
