using BankingSystemAssessment.Core.ErrorHandling;
using BankingSystemAssessment.Core.ErrorHandling.Extensions;
using BankingSystemAssessment.Web.Infrastructure.Extensions;
using BankingSystemAssessment.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace BankingSystemAssessment.Web.Filters
{
    /// <summary>
    /// Page exception filter which returns an JSON error response in Error Page
    /// </summary>
    public class PageExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<PageExceptionFilter> logger;

        public PageExceptionFilter(ILogger<PageExceptionFilter> logger)
        {
            this.logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            context.HttpContext.Request.Headers.TryGetValue(Constants.CorrelationIdHeaderKey, out var correlationId);

            if (context.Exception is PageException pageException)
            {
                ErrorResult result;
                if (pageException.ErrorResult != null)
                {
                    result = pageException.ErrorResult;
                }
                else
                {
                    result = new ErrorResult
                    {
                        CorrelationId = correlationId,
                        Exception = new ErrorItem
                        {
                            ErrorCode = pageException.StatusCode.ToString(),
                            ErrorMessage = "UnknownApiError"
                        }
                    };
                }

                context.Result = new ObjectResult(result)
                {
                    StatusCode = (int)pageException.StatusCode
                };
                context.Result = new RedirectToPageResult("/Error", new { errorResult = result });

                logger.Log(pageException.LogLevel(), context.Exception, "Api exception caught in the api filter. Returned with status code {StatusCode}", (int)pageException.StatusCode);
            }
            else
            {
                var errorResult = new ErrorResult
                {
                    CorrelationId = correlationId,
                    Exception = new ErrorItem
                    {
                        ErrorCode = "InternalServerError",
                        ErrorMessage = "UnknownApiError"
                    }
                };

                context.Result = new RedirectToPageResult("/Error", new { errorResult = errorResult });
                logger.LogError(context.Exception, "Internal Server exception caught in the api filter");
            }
            context.ExceptionHandled = true;
        }
    }
}