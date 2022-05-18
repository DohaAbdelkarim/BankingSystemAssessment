using BankingSystemAssessment.API.Models;
using System.Threading.Tasks;

namespace BankingSystemAssessment.API.Infrastructure.Services
{
    public interface ICustomerService
    {
        Task<CustomerDetailsModel> GetCustomerDetailsAsync(int customerId, int transactionsCount);
    }
}
