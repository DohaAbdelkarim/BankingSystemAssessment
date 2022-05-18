using System;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;
using Polly.Extensions.Http;
using Polly.Registry;
using Polly.Retry;
using Polly.Timeout;

namespace BankingSystemAssessment.Core.ErrorHandling.Extensions
{
    public static class PollyExtensions
    {
        public static IServiceCollection UsePollyPolicies(this IServiceCollection services, IConfiguration configuration)
        {
            var numberOfRetries = configuration.GetSection("Polly").GetValue("NumberOfRetries", 3);
            var timeoutInSecondsPerRetry = configuration.GetSection("Polly").GetValue("TimeoutInSecondsPerRetry", 5);
            var handledExceptionsBeforeBreak = configuration.GetSection("Polly").GetValue<int>("HandledExceptionsBeforeBreak", 3);
            var durationOfOpenCircuitInSeconds = configuration.GetSection("Polly").GetValue<int>("DurationOfOpenCircuitInSeconds", 60);

            var registry = new PolicyRegistry()
            {
                {
                    PolicyNames.DefaultRetry, HttpPolicyExtensions.HandleTransientHttpError().AddWaitAndRetryPolicy(numberOfRetries)
                },
                {
                    PolicyNames.DefaultTimeout, Policy.TimeoutAsync<HttpResponseMessage>(timeoutInSecondsPerRetry, TimeoutStrategy.Optimistic)
                },
                {
                    PolicyNames.DefaultCircuitBreaking, HttpPolicyExtensions.HandleTransientHttpError().AddCircuitBreakerPolicy(handledExceptionsBeforeBreak, durationOfOpenCircuitInSeconds)
                }
            };

            services.AddPolicyRegistry(registry);
            return services;
        }
        public static IHttpClientBuilder AddAllPolicies(this IHttpClientBuilder clientBuilder)
        {
            clientBuilder
                .AddPolicyHandlerFromRegistry(PolicyNames.DefaultRetry)
                .AddPolicyHandlerFromRegistry(PolicyNames.DefaultCircuitBreaking)
                .AddPolicyHandlerFromRegistry(PolicyNames.DefaultTimeout);
            return clientBuilder;
        }

        private static AsyncRetryPolicy<HttpResponseMessage> AddWaitAndRetryPolicy(this PolicyBuilder<HttpResponseMessage> policyBuilder, int numberOfRetries)
        {
            return policyBuilder
                .GetDefaultWaitAndRetryTypes()
                .BuildRetryPolicy(numberOfRetries);
        }

        private static PolicyBuilder<HttpResponseMessage> GetDefaultWaitAndRetryTypes(this PolicyBuilder<HttpResponseMessage> policyBuilder)
        {
            return policyBuilder
                    .Or<TimeoutRejectedException>()
                    .OrResult(c => c.StatusCode == HttpStatusCode.TooManyRequests);
        }

        private static AsyncRetryPolicy<HttpResponseMessage> BuildRetryPolicy(this PolicyBuilder<HttpResponseMessage> policyBuilder, int numberOfRetries)
        {
            var random = new Random();
            return policyBuilder.WaitAndRetryAsync(
                numberOfRetries,
                retryAttempt => TimeSpan.FromMilliseconds(random.Next(200, 1000)),
                (exception, timeSpan, retryCount, context) =>
                {
                    if (!context.TryGetLogger(out var logger))
                    {
                        return;
                    }

                    logger.LogWarning(exception.Exception, "Retrying request!");
                });
        }

        private static AsyncCircuitBreakerPolicy<HttpResponseMessage> AddCircuitBreakerPolicy(this PolicyBuilder<HttpResponseMessage> policyBuilder, int handledExceptionsBeforeBreak, int durationOfOpenCircuitInSeconds)
        {
            return policyBuilder.Or<TimeoutRejectedException>()
                        .CircuitBreakerAsync(
                            handledEventsAllowedBeforeBreaking: handledExceptionsBeforeBreak,
                            durationOfBreak: TimeSpan.FromSeconds(durationOfOpenCircuitInSeconds),
                            onBreak: (response, duration, context) =>
                            {
                                // All communication blocked
                                if (!context.TryGetLogger(out var logger))
                                {
                                    return;
                                }

                                logger.LogWarning($"Circuit open, all communication halted for {durationOfOpenCircuitInSeconds} seconds");
                            },
                    onReset: (context) =>
                    {
                        // Resume normal operation                                                
                        if (!context.TryGetLogger(out var logger))
                        {
                            return;
                        }

                        logger.LogWarning($"Circuit open, all communication halted for {durationOfOpenCircuitInSeconds} seconds");
                    });
        }
    }
}