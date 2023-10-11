using Core.IdentityServer.Services;
using Core.IdentityServer.Validation;
using Core.Accounts.DAL;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Core.Tenants.DAL;

namespace Core.IdentityServer.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<AccountsDbContext>();
            services.AddScoped<TenantsDbContext>();
            services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();
            services.AddScoped<IProfileService, ProfileService>();

            return services;
        }

        public static IServiceCollection AddIdentityServerService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentityServer()
                    .AddDeveloperSigningCredential() // this is for dev scenarios when you don’t have a certificate to use.
                    .AddInMemoryIdentityResources(Config.GetIdentityResources())
                    .AddInMemoryApiResources(Config.GetApiResources())
                    .AddInMemoryApiScopes(Config.GetApiScopes())
                    .AddInMemoryClients(Config.GetClients(configuration.GetSection("ClientApps")))
                    .AddProfileService<ProfileService>();

            return services;
        }
    }
}

