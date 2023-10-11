using Core.Accounts.DAL.Initializers;
using Core.Accounts.DAL;
using Core.Accounts.DAL.Seeders;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;

namespace Core.Accounts.API.Extensions
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

                var identityDbContext = services.GetRequiredService<AccountsDbContext>();
                // var env = services.GetRequiredService<IWebHostEnvironment>();

                //var dbInitializer = services.GetRequiredService<AccountsDatabaseInitializer>();
                //var dbSeed = services.GetRequiredService<AccountsDatabaseSeed>();

                Log.Information("Environment: " + Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));

                // identityDbContext.Database.EnsureDeleted();
                // identityDbContext.Database.EnsureCreated();

                identityDbContext.Database.Migrate();
                Log.Information("Database migrations executed.");


                //dbInitializer.Initialize();
                Log.Information("Database initialized with required data.");

                //dbSeed.Seed();
                Log.Information("Database seeded with test data.");
            }

            return webHost;
        }
    }
}

