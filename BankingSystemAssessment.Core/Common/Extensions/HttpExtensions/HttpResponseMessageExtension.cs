using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace BankingSystemAssessment.Core.Extensions.HttpExtensions
{
    public static class HttpResponseMessageExtension
    {
        public static async Task<T> DeserializeResponseAsync<T>(this HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseBody);
        }
    }
}