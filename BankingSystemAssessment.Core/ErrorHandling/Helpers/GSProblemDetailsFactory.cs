using BankingSystemAssessment.Core.ErrorHandling.Exceptions;
using BankingSystemAssessment.Core.ErrorHandling.Extensions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly.CircuitBreaker;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;

namespace BankingSystemAssessment.Core.ErrorHandling.Helpers
{
    public class GSProblemDetailsFactory : ProblemDetailsFactory
    {
        private readonly ApiBehaviorOptions _options;
        private ILogger<GSProblemDetailsFactory> _logger { get; }
        public GSProblemDetailsFactory(IOptions<ApiBehaviorOptions> options, ILogger<GSProblemDetailsFactory> logger)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override ProblemDetails CreateProblemDetails(
            HttpContext httpContext,
            int? statusCode = null,
            string title = null,
            string type = null,
            string detail = null,
            string instance = null)
        {
            statusCode ??= 500;

            var context = httpContext.Features.Get<IExceptionHandlerFeature>();

            ProblemDetails problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Type = type,
                Detail = detail,
                Instance = instance,
            };

            ApplyProblemDetailsDefaults(httpContext, problemDetails, statusCode.Value);

            if (context?.Error != null)
            {
                if (context.Error is NotFoundException notFoundException)
                {
                    statusCode = (int)notFoundException.StatusCode;
                    problemDetails.Status = statusCode;
                    //The result serializer doesn't use the status from the ProblemDetails object so we should set it manually.
                    httpContext.Response.StatusCode = statusCode.Value;
                    _logger.LogInformation($"NotFound exception caught in the middleware. Returned with status code {statusCode}.");
                }
                else if (context.Error is ApiException apiException)
                {
                    statusCode = (int)apiException.StatusCode;
                    //The result serializer doesn't use the status from the ProblemDetails object so we should set it manually.
                    httpContext.Response.StatusCode = statusCode.Value;
                    problemDetails.Status = statusCode;
                    if (!string.IsNullOrEmpty(apiException.ErrorCode))
                    {
                        problemDetails.Extensions.Add("errorcode", apiException.ErrorCode);
                    }
                    if (!string.IsNullOrEmpty(apiException.ErrorMessage))
                    {
                        problemDetails.Extensions.Add("errormessage", apiException.ErrorMessage);
                    }

                    using (_logger.BeginScope(JsonSerializer.Serialize(problemDetails)))
                    {
                        _logger.Log(apiException.LogLevel(), $"Api exception caught in the middleware. Returned with status code {(int)apiException.StatusCode}.");
                    }
                }
                else if (context.Error is BrokenCircuitException brokenCircuitException)
                {
                    var exception = (HttpRequestException)brokenCircuitException.InnerException;
                    using (_logger.BeginScope(JsonSerializer.Serialize(problemDetails)))
                    {
                        _logger.LogError(brokenCircuitException, $"brokenCircuitException exception caught in the middleware. with StatusCode: {(exception?.StatusCode != null ? (int)exception.StatusCode : -1)}," +
                            $"Error:{exception?.Message}. {brokenCircuitException.Message}.");
                    }
                }
                else //unhandled exception
                {
                    using (_logger.BeginScope(JsonSerializer.Serialize(problemDetails)))
                    {
                        _logger.LogError(context?.Error, $"Unhandled exception caught in the middleware. {context?.Error?.Message}.");
                    }
                }
            }
            return problemDetails;
        }

        public override ValidationProblemDetails CreateValidationProblemDetails(
            HttpContext httpContext,
            ModelStateDictionary modelStateDictionary,
            int? statusCode = null,
            string title = null,
            string type = null,
            string detail = null,
            string instance = null)
        {
            if (modelStateDictionary == null)
            {
                throw new ArgumentNullException(nameof(modelStateDictionary));
            }

            statusCode ??= 400;

            var problemDetails = new ValidationProblemDetails(modelStateDictionary)
            {
                Status = statusCode,
                Type = type,
                Detail = detail,
                Instance = instance,
            };

            if (title != null)
            {
                // For validation problem details, don't overwrite the default title with null.
                problemDetails.Title = title;
            }

            ApplyProblemDetailsDefaults(httpContext, problemDetails, statusCode.Value);

            using (_logger.BeginScope(JsonSerializer.Serialize(problemDetails)))
            {
                _logger.LogInformation($"Badrequest exception caught in the middleware. {JsonSerializer.Serialize(problemDetails.Errors)}");
            }
            return problemDetails;
        }

        private void ApplyProblemDetailsDefaults(HttpContext httpContext, ProblemDetails problemDetails, int statusCode)
        {
            problemDetails.Status = problemDetails.Status ?? statusCode;

            if (_options.ClientErrorMapping.TryGetValue(statusCode, out var clientErrorData))
            {
                problemDetails.Title ??= clientErrorData.Title;
                problemDetails.Type ??= clientErrorData.Link;
            }

            //Add traceIdentifier as extension for all responses
            var traceId = Activity.Current?.Id ?? httpContext?.TraceIdentifier;
            if (traceId != null)
            {
                problemDetails.Extensions["traceId"] = traceId;
            }

            //Add CorrelationId as extension for all responses
            Microsoft.Extensions.Primitives.StringValues correlationId = default;
            httpContext?.Request?.Headers.TryGetValue(Constants.CorrelationIdHeaderKey, out correlationId);
            problemDetails.Extensions[Constants.CorrelationIdHeaderKey] = correlationId.ToString();
        }
    }
}
