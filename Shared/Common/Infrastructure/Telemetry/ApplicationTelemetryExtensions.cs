using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Infrastructure.Telemetry
{
    /// <summary>
    /// Application Telemetry Extensions
    /// </summary>
    public static class ApplicationTelemetryExtensions
    {
        /// <summary>
        /// Add Application Insights Telemetry
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection RegisterApplicationTelemetry(this IServiceCollection services)
        {
            //Used for enabling/disabling specific metrics
            var options = new ApplicationInsightsServiceOptions();

            services.AddApplicationInsightsTelemetry(options);

            return services;
        }
    }
}
