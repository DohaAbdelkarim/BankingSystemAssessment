using System.Net.Http;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;
using BankingSystemAssessment.Web.Models;

namespace BankingSystemAssessment.Web.ApiClients
{
    public class BaseApiClient
    {
        private readonly HttpClient httpClient;
        private readonly IHttpContextAccessor httpContextAccessor;
        private const string CorrelationIdHeaderKey = "CorrelationId";

        private readonly JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };

        public BaseApiClient(IHttpContextAccessor httpContextAccessor, HttpClient httpClient)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.httpClient = httpClient;
        }

        public async Task<T> SendRequestAsync<T>(HttpMethod httpMethod, string url, object objectToSendInBody = null)
        {
            using (var httpRequestMessage = new HttpRequestMessage(httpMethod, url))
            {
                if (objectToSendInBody != null)
                {
                    var jsonContent = JsonSerializer.Serialize(objectToSendInBody, jsonSerializerOptions);
                    var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                    httpRequestMessage.Content = httpContent;
                    httpRequestMessage.Headers.TryAddWithoutValidation("Content-Type", "application/json");
                }
               
                var requestHeaders = httpContextAccessor.HttpContext.Request.Headers;
                var correlationId = Guid.NewGuid().ToString();
                if (requestHeaders.ContainsKey(CorrelationIdHeaderKey))
                {
                    correlationId = requestHeaders[CorrelationIdHeaderKey];
                }
                httpRequestMessage.Headers.Add(CorrelationIdHeaderKey, correlationId);

                using (var response = await httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false))
                {
                    using (var contentStream = await response.Content.ReadAsStreamAsync())
                    {
                        if (!response.IsSuccessStatusCode)
                        {
                            ErrorResult errorResult;
                            try
                            {
                                errorResult = await JsonSerializer.DeserializeAsync<ErrorResult>(contentStream, jsonSerializerOptions);
                            }
                            catch (JsonException)
                            {
                                throw new PageException(response.StatusCode);
                            }

                            switch (response.StatusCode)
                            {
                                case HttpStatusCode.NotFound:
                                    throw new PageException(HttpStatusCode.NotFound);
                                default:
                                    throw new PageException(response.StatusCode, errorResult);
                            }
                        }
                        return await JsonSerializer.DeserializeAsync<T>(contentStream, jsonSerializerOptions);
                    }
                }
            }
        }
    }
}