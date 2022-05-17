using BankingSystemAssessment.API.Models;
using System.Threading.Tasks;

namespace BankingSystemAssessment.API.Infrastructure.Services
{
    public interface IAccountService
    {
        Task<CreateAccountResponseModel> CreateAccountAsync(CreateAccountRequestModel createAccountRequestModel);
    }
}