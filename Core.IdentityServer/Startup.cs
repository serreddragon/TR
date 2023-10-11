using Core.Accounts.DAL;
using Core.IdentityServer.Extensions;
using Core.Tenants.DAL;
using HashidsNet;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Core.IdentityServer
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }

        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment,
                       IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.RegisterApplicationServices();

            services.AddDbContext<AccountsDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("IdentityDatabase")));
            services.AddDbContext<TenantsDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("TenantsDatabase")));

            services.AddIdentityServerService(Configuration);

            var hashIdsOptions = Configuration.GetSection("HashIds");
            services.AddSingleton<IHashids>(_ => new Hashids(hashIdsOptions["Salt"], Convert.ToInt32(hashIdsOptions["MinLength"])));
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }                        
            app.UseIdentityServer();
        }
    }
}
