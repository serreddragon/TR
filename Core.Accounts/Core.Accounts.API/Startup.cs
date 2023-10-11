using Common.Extensions;
using Common.Filters;
using Common.Infrastructure.HttpClient.Policies;
using Common.Infrastructure.Versioning;
using Common.ServiceBus.Configuration;
using Core.Accounts.API.Extensions;
using Core.Accounts.API.SwaggerSetup;
using Core.Accounts.DAL;
using Core.Accounts.DAL.Constants;
using Core.Accounts.Infrastructure.HttpClients;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Core.Accounts.API
{
#pragma warning disable CS1591
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.RegisterApplicationVersioning();

            services.AddControllers(opt =>
            {
                opt.Filters.Add(typeof(AuthenticationFilter));
                opt.Filters.Add(typeof(GlobalExceptionFilter));
                opt.Filters.Add(typeof(LogFilter));
            });

            services.AddHealthChecks()
                    .AddSqlServer(Configuration.GetConnectionString("AccountsDatabase"),
                name: "CoreAccountsDb",
                tags: new string[] { "Core.Accounts.API" });

            services.AddApiVersioning(
                options =>
                {
                    // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
                    options.ReportApiVersions = true;
                });

            services.AddVersionedApiExplorer(
                options =>
                {
                    // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                    // note: the specified format code will format the version as "'v'major[.minor][-status]"
                    options.GroupNameFormat = "'v'VVV";

                    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                    // can also be used to control the format of the API version in route templates
                    options.SubstituteApiVersionInUrl = true;
                });

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            services.AddDbContext<AccountsDbContext>(opts => opts.UseSqlServer(Configuration.GetConnectionString("AccountsDatabase")));

            services.AddSwaggerGen(
                options =>
                {
                    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer"
                    });

                    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                      {
                        {
                          new OpenApiSecurityScheme
                          {
                            Reference = new OpenApiReference
                              {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                              },
                              Scheme = "oauth2",
                              Name = "Bearer",
                              In = ParameterLocation.Header,

                            },
                            new List<string>()
                          }
                        });

                    // integrate xml comments
                    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
                }
                );

            services.RegisterHashIdAndAutoMapper(Configuration.GetSection("HashIds"));

            services.RegisterApplicationServices();

            services.RegisterCommonApplicationServices();

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                    .AddIdentityServerAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme,
                                                     options =>
                                                                {
                                                                    options.ApiName = AccountConstants.ApiName;
                                                                    options.Authority = Configuration["IdentityServer:Authority"];
                                                                });

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                  .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                  .RequireAuthenticatedUser()
                  .Build();
            });

            services.AddLocalization();
            services.AddHttpClient<ITenantsApi, TenantsApi>(client =>
            {
                client.BaseAddress = new Uri(Configuration["TenantApiConfiguration:Url"]);
            }).AddPolicyHandler(RetryPolicy.GetRetryPolicy(Convert.ToInt32(Configuration["TenantApiConfiguration:PolicyConfiguration:RetryCount"])))
              .AddPolicyHandler(CircuitBreakerPolicy.GetCircuitBreaker(Convert.ToInt32(Configuration["TenantApiConfiguration:PolicyConfiguration:RetryCount"]), Convert.ToInt32(Configuration["TenantApiConfiguration:PolicyConfiguration:BreakDuration"])));
            services.AddServiceBusServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(
                options =>
                {
                    // build a swagger endpoint for each discovered API version
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                    }
                });

            app.MigrateDatabase();
        }
    }
}
