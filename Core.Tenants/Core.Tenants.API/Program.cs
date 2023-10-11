using Core.Tenants.API;
using Serilog;

namespace Core.Catalog.API
{
#pragma warning disable CS1591
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            try
            {
               Log.Information("Starting Core.Tenants.API...");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
              Log.Fatal(ex, "Host terminated unexpectedly.");
            }
            finally
            {
             Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
