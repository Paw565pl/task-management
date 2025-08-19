using TaskManagement.Backend.Features.Projects.Services;
using TaskManagement.Backend.Features.Tasks.Services;

namespace TaskManagement.Backend.Core.Extensions;

public static class AddAppServicesExtensions
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddScoped<ProjectService>();
        services.AddScoped<TaskService>();

        return services;
    }
}
