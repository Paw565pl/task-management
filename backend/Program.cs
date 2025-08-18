using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using TaskManagement.Backend.Core.DbContext;
using TaskManagement.Backend.Core.ExceptionHandler;
using TaskManagement.Backend.Core.Extensions;
using TaskManagement.Backend.Features.Auth.OpenApi;
using TaskManagement.Backend.Features.Auth.Options;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services.AddOptions<AuthOptions>()
    .Bind(builder.Configuration.GetSection(AuthOptions.SectionName))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddDbContextPool<AppDbContext>(optionsBuilder =>
    optionsBuilder.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"))
);
builder.Services.AddOpenApi(options =>
    options.AddDocumentTransformer<JwtBearerOpenApiDocumentTransformer>()
);

builder.Services.AddControllers().MapBindingErrorsToProblemDetails();
builder.Services.AddProblemDetails();
builder.Services.AddHealthChecks().AddDbContextCheck<AppDbContext>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder
    .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var authSettings = builder
            .Configuration.GetSection(AuthOptions.SectionName)
            .Get<AuthOptions>();

        options.Authority = authSettings?.Authority;
        options.Audience = authSettings?.Audience;

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
        };

        if (builder.Environment.IsDevelopment())
        {
            options.RequireHttpsMetadata = false;
#pragma warning disable CA5404
            tokenValidationParameters.ValidateIssuer = false;
#pragma warning restore CA5404
        }

        options.TokenValidationParameters = tokenValidationParameters;
    });
builder.Services.AddAuthorization();

builder.Services.AddAppServices();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
    if (pendingMigrations.Any())
    {
        Console.WriteLine("Applying pending migrations...");
        await dbContext.Database.MigrateAsync();
        Console.WriteLine("Migrations applied successfully.");
    }
}

app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
        options
            .AddPreferredSecuritySchemes(nameof(SecuritySchemeType.OAuth2))
            .AddAuthorizationCodeFlow(
                nameof(SecuritySchemeType.OAuth2),
                flow =>
                {
                    flow.ClientId = "scalar";
                    flow.RedirectUri = "http://localhost:5000/scalar/callback";
                    flow.SelectedScopes = ["openid", "profile", "email"];
                    flow.Pkce = Pkce.Sha256;
                }
            )
    );
}

app.MapHealthChecks("/api/_health");
app.MapControllers();

await app.RunAsync();
