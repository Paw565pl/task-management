using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using TaskManagement.Backend.Core.Context;
using TaskManagement.Backend.Core.ExceptionHandler;
using TaskManagement.Backend.Features.Auth.Settings;
using TaskManagement.Backend.Features.Project.Service;
using TaskManagement.Backend.Features.Task.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextPool<AppDbContext>(optionsBuilder =>
    optionsBuilder.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options => options.AddDocumentTransformer((document, _, _) =>
{
    document.Components ??= new OpenApiComponents();

    document.Components.SecuritySchemes[JwtBearerDefaults.AuthenticationScheme] = new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        BearerFormat = "JWT",
    };

    document.SecurityRequirements.Add(new OpenApiSecurityRequirement
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
    });

    return Task.CompletedTask;
}));

builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddHealthChecks().AddDbContextCheck<AppDbContext>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddOptions<AuthSettings>()
    .Bind(builder.Configuration.GetSection(AuthSettings.SectionName))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    var authSettings = builder.Configuration.GetSection(AuthSettings.SectionName).Get<AuthSettings>();

    options.Authority = authSettings?.Authority;
    options.Audience = authSettings?.Audience;

    var tokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
    };

    if (builder.Environment.IsDevelopment())
    {
        options.RequireHttpsMetadata = false;
        tokenValidationParameters.ValidateIssuer = false;
    }

    options.TokenValidationParameters = tokenValidationParameters;
});
builder.Services.AddAuthorization();

builder.Services.AddScoped<ProjectService>();
builder.Services.AddScoped<TaskService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    var pendingMigrations = dbContext.Database.GetPendingMigrations();
    if (pendingMigrations.Any())
    {
        Console.WriteLine("Applying pending migrations...");
        dbContext.Database.Migrate();
        Console.WriteLine("Migrations applied successfully.");
    }
}

app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
        options
            .AddPreferredSecuritySchemes(JwtBearerDefaults.AuthenticationScheme)
            .AddAuthorizationCodeFlow(JwtBearerDefaults.AuthenticationScheme, flow =>
            {
                var authSection = builder.Configuration.GetSection(AuthSettings.SectionName);

                flow.AuthorizationUrl =
                    (authSection[nameof(AuthSettings.Authority)] + "/protocol/openid-connect/auth").Replace("keycloak",
                        "localhost");
                flow.TokenUrl =
                    (authSection[nameof(AuthSettings.Authority)] + "/protocol/openid-connect/token").Replace("keycloak",
                        "localhost");
                flow.RefreshUrl =
                    (authSection[nameof(AuthSettings.Authority)] + "/protocol/openid-connect/token").Replace("keycloak",
                        "localhost");
                flow.ClientId = "scalar";
                flow.RedirectUri = "http://localhost:5000/scalar/callback";
                flow.SelectedScopes = ["openid", "profile", "email"];
                flow.Pkce = Pkce.Sha256;
            })
    );
}

app.MapHealthChecks("/api/_health");
app.MapControllers();

app.Run();
