using EntityFramework.Exceptions.PostgreSQL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Scalar.AspNetCore;
using TaskManagement.Backend.Core.Db;
using TaskManagement.Backend.Core.ExceptionHandlers;
using TaskManagement.Backend.Core.Extensions;
using TaskManagement.Backend.Features.Auth.OpenApi;
using TaskManagement.Backend.Features.Auth.Options;
using TaskManagement.Backend.Features.Auth.Policies;
using ZiggyCreatures.Caching.Fusion;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services.AddOptions<AuthOptions>()
    .Bind(builder.Configuration.GetSection(AuthOptions.SectionName))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddDbContextPool<AppDbContext>(optionsBuilder =>
    optionsBuilder
        .UseNpgsql(builder.Configuration.GetConnectionString("Postgres"))
        .UseExceptionProcessor()
);
builder.Services.AddOpenTelemetrySetup(
    new Uri(builder.Configuration.GetConnectionString("Jaeger") ?? string.Empty)
);
builder.Services.AddOpenApi(options =>
    options.AddDocumentTransformer<JwtBearerOpenApiDocumentTransformer>()
);

builder.Services.AddControllers().MapBindingErrorsToProblemDetails();
builder.Services.AddProblemDetails(options =>
    options.CustomizeProblemDetails = context =>
    {
        context.ProblemDetails.Instance =
            $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
    }
);
builder.Services.AddHealthChecks().AddDbContextCheck<AppDbContext>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder
    .Services.AddFusionCache()
    .WithDefaultEntryOptions(options =>
    {
        options.Duration = TimeSpan.FromMinutes(5);

        options.IsFailSafeEnabled = true;
        options.FailSafeMaxDuration = TimeSpan.FromHours(1);
        options.FailSafeThrottleDuration = TimeSpan.FromSeconds(30);

        options.FactorySoftTimeout = TimeSpan.FromMilliseconds(200);
        options.FactoryHardTimeout = TimeSpan.FromMilliseconds(3000);
    });

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
builder.Services.AddAuthorization(options => options.AddAdminPolicy());

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

    if (app.Environment.IsDevelopment())
        await DataSeeder.SeedAsync(dbContext);
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
