using BankingSystemAssessment.Core.ErrorHandling;
using Microsoft.AspNetCore.Http;

namespace BankingSystemAssessment.Web.Infrastructure
{
    public class CorrelationIdResolver : ICorrelationIdResolver
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public CorrelationIdResolver(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public string GetCorrelationId()
        {
            httpContextAccessor.HttpContext?.Request?.Headers?.TryGetValue(Constants.CorrelationIdHeaderKey, out var correlationId);
            return correlationId;
        }
    }
}