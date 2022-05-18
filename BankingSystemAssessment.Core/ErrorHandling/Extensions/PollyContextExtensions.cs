using Microsoft.Extensions.Logging;
using Polly;

namespace BankingSystemAssessment.Core.ErrorHandling.Extensions
{
    public static class PollyContextExtensions
    {
        private static readonly string LoggerKey = "ILogger";

        public static Context WithLogger<T>(this Context context, ILogger logger)
        {
            context[LoggerKey] = logger;
            return context;
        }

        public static bool TryGetLogger(this Context context, out ILogger logger)
        {
            if (context.TryGetValue(LoggerKey, out var loggerObject) && loggerObject is ILogger theLogger)
            {
                logger = theLogger;
                return true;
            }

            logger = null;
            return false;
        }
    }
}