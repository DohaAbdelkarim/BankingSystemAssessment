using BankingSystemAssessment.Web.ApiClients.Contracts;
using BankingSystemAssessment.Web.Models;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Threading.Tasks;

namespace BankingSystemAssessment.Web.ApiClients
{
    public class AccountApiClient : BaseApiClient, IAccountApiClient
    {
        private const string BaseUrl = "http://localhost:5000/api/Account";

        public AccountApiClient(IHttpContextAccessor httpContextAccessor, HttpClient httpClient)
          : base(httpContextAccessor, httpClient)
        {
        }

        public async Task<CreateAccountResponseModel> CreateAccountAsync(CreateAccountRequestModel createAccountRequestModel)
        {
            var createGuaranteeTypeResponse = await SendRequestAsync<CreateAccountResponseModel>(HttpMethod.Post, BaseUrl, createAccountRequestModel);
            return createGuaranteeTypeResponse;
        }
    } 
}