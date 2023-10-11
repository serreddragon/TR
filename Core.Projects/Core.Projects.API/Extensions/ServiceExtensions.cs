using Core.Projects.DAL;
using Core.Projects.Service.Activities.Command;
using Core.Projects.Service.ActivityGroups.Command;
using Core.Projects.Service.ProjectPhases.Command;
using Core.Projects.Service.Projects.Command;

namespace Core.Projects.API.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection RegisterProjectServices(this IServiceCollection services)
        {
            services.AddScoped<ProjectsDbContext>();

            services.AddScoped<AddProjectCommandValidator>();
            services.AddScoped<UpdateProjectCommandValidator>();

            services.AddScoped<AddProjectPhaseCommandValidator>();
            services.AddScoped<UpdateProjectPhaseCommandValidator>();

            services.AddScoped<ActivityGroupCommandValidator>();

            services.AddScoped<ActivityCommandValidator>();

            return services;
        }
    }
}
