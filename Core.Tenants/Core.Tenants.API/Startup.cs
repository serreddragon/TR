using Common.Extensions;
using Common.Filters;
using Common.Infrastructure.HttpClient.Policies;
using Common.Infrastructure.Versioning;
using Core.Tenants.API.Extensions;
using Core.Tenants.API.SwaggerSetup;
using Core.Tenants.DAL;
using Core.Tenants.DAL.Constants;
using Core.Tenants.Infrastructure.HttpClients;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Core.Tenants.API
{
#pragma warning disable CS1591
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry();
            services.RegisterApplicationVersioning();

            services.AddControllers(opt =>
            {
                opt.Filters.Add(typeof(AuthenticationFilter));
                opt.Filters.Add(typeof(GlobalExceptionFilter));
                opt.Filters.Add(typeof(LogFilter));
            });

            services.AddHealthChecks()
                    .AddSqlServer(Configuration.GetConnectionString("TenantDatabase"),
                name: "CoreTenantDb",
                tags: new string[] { "Core.Tenant.API" });

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            services.AddDbContext<TenantsDbContext>(opts => opts.UseSqlServer(Configuration.GetConnectionString("TenantDatabase")));

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
                                                         options.ApiName = TenantConstants.ApiName;
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

            var s = Convert.ToInt32(Configuration["AccountApiConfiguration:PolicyConfiguration:RetryCount"]);

            services.AddHttpClient<IAccountsApi, AccountsApi>(client =>
            {
                client.BaseAddress = new Uri(Configuration["AccountApiConfiguration:Url"]);
            }).AddPolicyHandler(RetryPolicy.GetRetryPolicy(Convert.ToInt32(Configuration["AccountApiConfiguration:PolicyConfiguration:RetryCount"])))
               .AddPolicyHandler(CircuitBreakerPolicy.GetCircuitBreaker(Convert.ToInt32(Configuration["AccountApiConfiguration:PolicyConfiguration:RetryCount"]), Convert.ToInt32(Configuration["AccountApiConfiguration:PolicyConfiguration:BreakDuration"])));
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

