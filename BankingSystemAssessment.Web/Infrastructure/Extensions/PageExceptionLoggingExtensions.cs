using BankingSystemAssessment.Web.Models;
using Microsoft.Extensions.Logging;

namespace BankingSystemAssessment.Web.Infrastructure.Extensions
{
    public static class PageExceptionLoggingExtensions
    {
        public static LogLevel LogLevel(this PageException pageException)
        {
            return (int)pageException.StatusCode >= 500 ? Microsoft.Extensions.Logging.LogLevel.Error : Microsoft.Extensions.Logging.LogLevel.Information;
        }
    }
}