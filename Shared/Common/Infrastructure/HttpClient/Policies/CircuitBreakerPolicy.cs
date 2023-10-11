using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;

namespace Common.Infrastructure.HttpClient.Policies
{
    public class CircuitBreakerPolicy
    {
        public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreaker(int retryCount, int breakDuration)
        {
            return HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(retryCount, TimeSpan.FromSeconds(breakDuration));
        }

    }
}
