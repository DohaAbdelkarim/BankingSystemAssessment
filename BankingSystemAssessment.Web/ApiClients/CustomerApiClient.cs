using BankingSystemAssessment.Core.Common.Models;
using BankingSystemAssessment.Web.ApiClients.Contracts;
using BankingSystemAssessment.Web.Models;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Threading.Tasks;

namespace BankingSystemAssessment.Web.ApiClients
{
    public class CustomerApiClient : BaseApiClient, ICustomerApiClient
    {
        private const string BaseUrl = "http://localhost:5000/api/Customer";
        
        public CustomerApiClient(IHttpContextAccessor httpContextAccessor, HttpClient httpClient)
          : base(httpContextAccessor, httpClient)
        {
        }

        public async Task<PagedResponse<CustomerIndexModel>> GetCustomersAsync()
        {
            return await SendRequestAsync<PagedResponse<CustomerIndexModel>>(HttpMethod.Get, BaseUrl).ConfigureAwait(false);
        }
        
        public async Task<CustomerDetailsModel> GetCustomerDetailsAsync(int customerId)
        {
            return await SendRequestAsync<CustomerDetailsModel>(HttpMethod.Get, BaseUrl+$"/{customerId}").ConfigureAwait(false);
        }
    }
}