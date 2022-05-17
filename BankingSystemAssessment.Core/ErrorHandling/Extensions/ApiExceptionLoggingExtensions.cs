using BankingSystemAssessment.Core.ErrorHandling.Exceptions;
using Microsoft.Extensions.Logging;

namespace BankingSystemAssessment.Core.ErrorHandling.Extensions
{
    public static class ApiExceptionLoggingExtensions
    {
        public static LogLevel LogLevel(this ApiException apiException)
        {
            return (int)apiException.StatusCode >= 500 ? Microsoft.Extensions.Logging.LogLevel.Error : Microsoft.Extensions.Logging.LogLevel.Information;
        }
    }
}