using Core.Tenants.DAL;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Core.Tenants.API.Extensions
{
    /// <summary>
    ///Application Builder Extensions
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        ///Migrate Database
        /// </summary>
        public static IApplicationBuilder MigrateDatabase(this IApplicationBuilder webHost)
        {
            var serviceScopeFactory = (IServiceScopeFactory)webHost.ApplicationServices.GetService(typeof(IServiceScopeFactory));

            using (var scope = serviceScopeFactory.CreateScope())
            {
                var services = scope.ServiceProvider;

                var tenantDbContext = services.GetRequiredService<TenantsDbContext>();

                Log.Information("Environment: " + Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
                       
                tenantDbContext.Database.Migrate();
                Log.Information("Database migrations executed.");
            }

            return webHost;
        }
    }
}

