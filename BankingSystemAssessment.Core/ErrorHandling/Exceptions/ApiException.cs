using System;
using System.Net;

namespace BankingSystemAssessment.Core.ErrorHandling.Exceptions
{
    public class ApiException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }

        public ApiException() { }
        public ApiException(HttpStatusCode statusCode) : this(statusCode, null, null) { }
        public ApiException(HttpStatusCode statusCode, string errorCode)
        {
            StatusCode = statusCode;
            ErrorCode = errorCode;
        }
        public ApiException(HttpStatusCode statusCode, string errorCode, string errorMessage)
        {
            StatusCode = statusCode;
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }
    }
}