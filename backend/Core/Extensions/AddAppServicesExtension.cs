using TaskManagement.Backend.Features.Project.Service;
using TaskManagement.Backend.Features.Task.Service;

namespace TaskManagement.Backend.Core.Extensions;

public static class AddAppServicesExtension
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddScoped<ProjectService>();
        services.AddScoped<TaskService>();

        return services;
    }
}
