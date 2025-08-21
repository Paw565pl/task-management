using Npgsql;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace TaskManagement.Backend.Core.Extensions;

public static class OpenTelemetryExtensions
{
    public static IServiceCollection AddOpenTelemetrySetup(
        this IServiceCollection services,
        Uri otlpExporter
    )
    {
        services
            .AddOpenTelemetry()
            .ConfigureResource(builder => builder.AddService("TaskManagement.Backend"))
            .WithTracing(builder =>
                builder
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddEntityFrameworkCoreInstrumentation()
                    .AddNpgsql()
                    .AddOtlpExporter(options => options.Endpoint = otlpExporter)
            );

        return services;
    }
}
