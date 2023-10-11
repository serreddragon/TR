using Core.Projects.API.Extensions;
using Core.Projects.DAL;
using Core.Projects.Service.Activities;
using Core.Projects.Service.ActivityGroups;
using Core.Projects.Service.Interfaces;
using Core.Projects.Service.ProjectPhases;
using Core.Projects.Service.Projects;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Core.Projects.API
{
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
            services.AddControllers();
            services.AddMvc();

            services.AddScoped<IProjectsService, ProjectsService>();
            services.AddScoped<IProjectPhasesService, ProjectPhasesService>();
            services.AddScoped<IActivityGroupsService, ActivityGroupsService>();
            services.AddScoped<IActivitiesService, ActivitiesService>();

            services.AddDbContext<ProjectsDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ProjectsDatabase")));

            //services.AddAutoMapper(typeof(Startup));  
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            /*
            services.AddHealthChecks()
                    .AddSqlServer(Configuration.GetConnectionString("ProjectsDatabase"),
                name: "TR.Core.Projects");*/

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

            // services.AddTransient<IConfiguration, Configuration>();

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

            services.RegisterProjectServices();

            // services.RegisterHashIdAndAutoMapper(Configuration.GetSection("HashIds"));

            //services.RegisterApplicationServices();
            /*
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
            });*/

            services.AddLocalization();

            //  services.AddServiceBusServices();
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

            //   app.UseAuthentication();

            //   app.UseAuthorization();

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

            app.Build();
            //  app.MigrateDatabase();
        }
    }
}
