using BankingSystemAssessment.API.Infrastructure.Domain;
using BankingSystemAssessment.API.Models;
using BankingSystemAssessment.Core.Common.Models;
using System.Threading.Tasks;

namespace BankingSystemAssessment.API.Infrastructure.Services
{
    public interface ITransactionService
    {
        Task<Transaction> CreateDepositAsync(Account account, decimal credit);
        Task<PagedResponse<TransactionModel>> GetTransactionsAsync(int accountId, FilterModel filterModel, string transactionDateFrom = null, string transactionDateTo = null);
    }
}