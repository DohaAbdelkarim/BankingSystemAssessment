using System;
using System.Net;

namespace BankingSystemAssessment.Web.Models
{
    public class PageException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }

        public ErrorResult ErrorResult { get; set; }

        public PageException() { }

        public PageException(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }

        public PageException(HttpStatusCode statusCode, ErrorResult errorResult)
        {
            StatusCode = statusCode;
            ErrorResult = errorResult;
        }
    }
}