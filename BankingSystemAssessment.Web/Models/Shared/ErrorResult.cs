using System.Collections.Generic;
using System.Text.Json;

namespace BankingSystemAssessment.Web.Models
{
    public class ErrorResult
    {
        public string Title { get; set; }

        public string CorrelationId { get; set; }

        public string TraceId { get; set; }

        public ErrorItem Exception { get; set; }

        public IDictionary<string, string[]> Errors { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
    public class ErrorItem
    {
        public string ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}