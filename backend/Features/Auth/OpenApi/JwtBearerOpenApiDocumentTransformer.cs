using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using TaskManagement.Backend.Features.Auth.Options;

namespace TaskManagement.Backend.Features.Auth.OpenApi;

public class JwtBearerOpenApiDocumentTransformer(
    IApiDescriptionGroupCollectionProvider apiDescriptionGroupCollectionProvider,
    IOptions<AuthOptions> authSettings,
    IWebHostEnvironment environment) : IOpenApiDocumentTransformer
{
    public System.Threading.Tasks.Task TransformAsync(OpenApiDocument document,
        OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken)
    {
        var authorityUrl = environment.IsDevelopment() ? authSettings.Value.Authority
                .Replace("keycloak", "localhost") : authSettings.Value.Authority;
        var authorizationUrl = authorityUrl + "/protocol/openid-connect/auth";
        var tokenUrl = authorityUrl + "/protocol/openid-connect/token";
        var refreshUrl = authorityUrl + "/protocol/openid-connect/token";

        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes[nameof(SecuritySchemeType.OAuth2)] = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.OAuth2,
            Scheme = JwtBearerDefaults.AuthenticationScheme,
            BearerFormat = "JWT",
            Flows = new OpenApiOAuthFlows
            {
                AuthorizationCode = new OpenApiOAuthFlow
                {
                    AuthorizationUrl = new Uri(authorizationUrl),
                    TokenUrl = new Uri(tokenUrl),
                    RefreshUrl = new Uri(refreshUrl),
                }
            }
        };

        var securedEndpoints = new HashSet<(string path, string method)>();
        var securityRequirement = new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = JwtBearerDefaults.AuthenticationScheme
                    }
                },
                []
            }
        };

        // construct securedEndpoints 
        foreach (var apiDescriptionGroup in apiDescriptionGroupCollectionProvider.ApiDescriptionGroups.Items)
        {
            foreach (var apiDescription in apiDescriptionGroup.Items)
            {
                if (string.IsNullOrEmpty(apiDescription.RelativePath) ||
                    string.IsNullOrEmpty(apiDescription.HttpMethod))
                    continue;

                var metadata = apiDescription.ActionDescriptor.EndpointMetadata;
                var hasAuthorizeAttribute = metadata.OfType<AuthorizeAttribute>().Any();
                var hasAllowAnonymousAttribute = metadata.OfType<AllowAnonymousAttribute>().Any();

                if (!hasAuthorizeAttribute || hasAllowAnonymousAttribute) continue;

                var path = apiDescription.RelativePath.Trim('/');
                var method = apiDescription.HttpMethod;
                securedEndpoints.Add((path, method));
            }
        }

        // apply security requirements only to matching endpoints
        foreach (var (pathKey, pathItem) in document.Paths)
        {
            foreach (var (operationType, openApiOperation) in pathItem.Operations)
            {
                var path = pathKey.Trim('/');
                var method = operationType.ToString().ToUpperInvariant();

                if (!securedEndpoints.Contains((path, method)))
                    continue;

                openApiOperation.Security ??= [];
                openApiOperation.Security.Add(securityRequirement);
            }
        }

        return System.Threading.Tasks.Task.CompletedTask;
    }
}
