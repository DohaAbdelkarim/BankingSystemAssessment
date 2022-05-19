using BankingSystemAssessment.Web.Models;
using System.Threading.Tasks;

namespace BankingSystemAssessment.Web.ApiClients.Contracts
{
    public interface IAccountApiClient
    {
        Task<CreateAccountResponseModel> CreateAccountAsync(CreateAccountRequestModel createAccountRequestModel);
    }
}