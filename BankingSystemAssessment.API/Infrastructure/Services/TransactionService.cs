using BankingSystemAssessment.API.Infrastructure.Domain;
using BankingSystemAssessment.API.Infrastructure.Enums;
using BankingSystemAssessment.API.Infrastructure.Helpers;
using BankingSystemAssessment.Core.ErrorHandling.Exceptions;
using BankingSystemAssessment.Core.Extensions.DatabaseExtensions;
using BankingSystemAssessment.Core.Logging;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace BankingSystemAssessment.API.Infrastructure.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly BankingSystemContext _context;
        private readonly ILogger<TransactionService> _logger;

        public TransactionService(BankingSystemContext context, ILogger<TransactionService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Transaction> CreateDepositAsync(Account account, decimal credit)
        {
            if (account == null || account.Status != AccountStatus.Active.ToString())
            {
                _logger.LogInformation(LogEvents.InsertItem, $"BadRequest Error.Deposit can't be made. Invalid account");
                throw new ApiException(HttpStatusCode.BadRequest, TransactionErrorCodes.InvalidAccount.ToString());
            }

            var deposit = new Transaction
            {
                AccountId = account.Id,
                ReferenceNumber = TransactionReferenceNumberHelper.GenerateTransactionReferenceNumber(),
                Description = $"Deposit {credit} {account.Currency}",
                TransactionDate = DateTimeOffset.Now,
                Credit = credit,
                Debit = 0,
                TranscationType = TranscationType.Deposit.ToString(),
                BalanceAfter = account.Balance + credit
            };

            //update customer account
            deposit.Account = account;
            deposit.Account.Balance += deposit.Credit;

            await _context.PostAsync(deposit, _logger);

            _logger.LogInformation($"Transaction {deposit.ReferenceNumber} of type {deposit.TranscationType} is made for account number: {account.AccountNumber} successfully.");
            return deposit;
        }
    }
}