using BankingSystemAssessment.API.Infrastructure.Services;
using BankingSystemAssessment.API.Models;
using BankingSystemAssessment.Core.Common.Models;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BankingSystemAssessment.API.UseCases.Transaction
{
    public class TransactionPagedQueryHandler : IRequestHandler<TransactionPagedQuery, PagedResponse<TransactionModel>>
    {
        private readonly ITransactionService _transactionService;

        public TransactionPagedQueryHandler(ITransactionService transactionService)
        {
            _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
        }
        public async Task<PagedResponse<TransactionModel>> Handle(TransactionPagedQuery request, CancellationToken cancellationToken)
        {
            return await _transactionService.GetTransactionsAsync(request.AccountId, request.FilterModel, request.TransactionDateFrom,request.TransactionDateTo);
        }
    }
}