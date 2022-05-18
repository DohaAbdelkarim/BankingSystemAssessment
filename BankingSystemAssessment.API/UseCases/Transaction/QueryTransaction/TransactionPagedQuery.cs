using BankingSystemAssessment.API.Models;
using BankingSystemAssessment.Core.Common.Models;
using MediatR;

namespace BankingSystemAssessment.API.UseCases.Transaction
{
    public class TransactionPagedQuery : IRequest<PagedResponse<TransactionModel>>
    {
        public int AccountId { get; }
        public FilterModel FilterModel { get; }
        public string TransactionDateFrom { get; }
        public string TransactionDateTo { get; }

        public TransactionPagedQuery(int accountId, int pageIndex, int pageSize, string transactionDateFrom, string transactionDateTo, string searchString, string sortOrder, string sortProperty)
        {
            AccountId = accountId;
            TransactionDateFrom = transactionDateFrom;
            TransactionDateTo = transactionDateTo;
            FilterModel = new FilterModel()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                SearchString = searchString,
                SortOrder = sortOrder,
                SortProperty = sortProperty
            };
        }
    }
}