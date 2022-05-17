using System;
using System.Net;

namespace BankingSystemAssessment.Core.ErrorHandling.Exceptions
{
    public class NotFoundException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public NotFoundException()
        {
            StatusCode = HttpStatusCode.NotFound;
        }
    }
}