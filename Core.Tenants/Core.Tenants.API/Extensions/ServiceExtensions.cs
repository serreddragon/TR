using AutoMapper;
using Common.Interface;
using Core.Tenants.DAL;
using Core.Tenants.Infrastructure.HttpClients;
using Core.Tenants.Service.AccountTenantMembership;
using Core.Tenants.Service.AccountTenantMembership.Command;
using Core.Tenants.Service.Common;
using Core.Tenants.Service.Interfaces;
using Core.Tenants.Service.MapperProfile;
using Core.Tenants.Service.Tenant;
using Core.Tenants.Service.Tenant.Command;
using HashidsNet;

namespace Core.Tenants.API.Extensions
{
    /// <summary>
    /// Service Extensions
    /// </summary>s
    public static class ServiceExtensions
    {
        /// <summary>
        /// Register Application Services
        /// </summary>s

        public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<TenantsDbContext>();
            services.AddScoped<IDatabaseContext>(x => x.GetRequiredService<TenantsDbContext>());

            services.AddScoped<IAccountTenantMembershipService, AccountTenantMembershipService>();
            services.AddScoped<ITenantService, TenantService>();

            services.AddScoped<CreateTenantCommandValidator>();
            services.AddScoped<UpdateTenantCommandValidator>();
            services.AddScoped<TenantValidations>();

            services.AddScoped<CreateAccountTenantMembershipCommandValidator>();
            services.AddScoped<UpdateAccountTenantMembershipCommandValidator>();

            services.AddScoped<IAccountsApi, AccountsApi>();
            
            return services;
        }

        /// <summary>
        /// Register AutoMapper
        /// </summary>s
        public static IServiceCollection RegisterHashIdAndAutoMapper(this IServiceCollection services, IConfigurationSection hashIdsOptions)
        {
            var hashIds = new Hashids(hashIdsOptions["Salt"],
                                      Convert.ToInt32(hashIdsOptions["MinLength"]));

            services.AddSingleton<IHashids>(_ => hashIds);

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MapperProfile(hashIds));
            });
            IMapper mapper = mapperConfig.CreateMapper();

            services.AddSingleton(mapper);

            return services;
        }
    }
}

