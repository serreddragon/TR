using AutoMapper;
using Common.Interface;
using Core.Accounts.DAL;
using Core.Accounts.DAL.Repositories.Implementations;
using Core.Accounts.DAL.Repositories.Interface;
using Core.Accounts.Infrastructure.HttpClients;
using Core.Accounts.Service;
using Core.Accounts.Service.Accounts;
using Core.Accounts.Service.Accounts.Command;
using Core.Accounts.Service.Common;
using Core.Accounts.Service.Interface;
using Core.Accounts.Service.MapperProfile;
using Core.Tenants.Service.Tenant.Command;
using HashidsNet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Core.Accounts.API.Extensions
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
            services.AddScoped<AccountsDbContext>();
            services.AddScoped<IDatabaseContext>(x => x.GetRequiredService<AccountsDbContext>());

            //services.AddScoped<AccountsDatabaseInitializer>();
            //services.AddScoped<AccountsDatabaseSeed>();
            //services.AddScoped<RolesInitializer>();

            #region Services    
            services.AddScoped<IAccountsService, AccountsService>();
            services.AddScoped<IRolesService, RolesService>();
            services.AddScoped<ITenantsApi, TenantsApi>();

            #region Validators
            services.AddScoped<RegisterAccountCommandValidator>();
            services.AddScoped<UpdateAccountCommandValidator>();
            services.AddScoped<EmailVerificationValidator>();
            services.AddScoped<ChangePasswordValidator>();
            services.AddScoped<CreateRoleCommandValidator>();
            services.AddScoped<AssignAccountTenantRolesCommandValidator>();
            services.AddScoped<CreateAccountRolesCommandValidator>();
            services.AddScoped<AuthValidations>();
            #endregion 

            return services;
        }

        /// <summary>
        /// Register Reposiories
        /// </summary>
        public static IServiceCollection RegisterReposiories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

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

#endregion