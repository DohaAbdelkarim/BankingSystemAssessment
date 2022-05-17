using BankingSystemAssessment.API.Infrastructure.Domain;
using System.Threading.Tasks;

namespace BankingSystemAssessment.API.Infrastructure.Services
{
    public interface ITransactionService
    {
        Task<Transaction> CreateDepositAsync(Account account, decimal credit);
    }
}