using AutoMapper;
using BankingSystemAssessment.API.Infrastructure.Domain;
using BankingSystemAssessment.API.Infrastructure.Enums;
using BankingSystemAssessment.API.Infrastructure.Helpers;
using BankingSystemAssessment.API.Models;
using BankingSystemAssessment.Core.Common.Models;
using BankingSystemAssessment.Core.ErrorHandling.Exceptions;
using BankingSystemAssessment.Core.Extensions.DatabaseExtensions;
using BankingSystemAssessment.Core.Extensions.EnumerableExtensions;
using BankingSystemAssessment.Core.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;

namespace BankingSystemAssessment.API.Infrastructure.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly BankingSystemContext _context;
        private readonly ILogger<TransactionService> _logger;
        private readonly IMapper _mapper;
        private readonly IValidationService _validationService;

        public TransactionService(BankingSystemContext context, ILogger<TransactionService> logger, IMapper mapper, IValidationService validationService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _validationService = validationService ?? throw new ArgumentNullException(nameof(validationService));
        }

        public async Task<Transaction> CreateDepositAsync(Account account, decimal credit, bool initialCredit = false)
        {
            if (initialCredit)//Validation in case the account isn't created yet
            {
                if (account == null || account.Status != AccountStatus.Active.ToString())
                {
                    _logger.LogInformation(LogEvents.InsertItem, $"BadRequest Error.Deposit can't be made. Invalid account");
                    throw new ApiException(HttpStatusCode.BadRequest, TransactionErrorCodes.InvalidAccountForDepositTransaction.ToString());
                }
            }
            else
            {
                //validate that the account exists and active
                await _validationService.ValidateAccountAsync(account.Id);
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

        private void ParseSearchTransactionDates(ref DateTime? searchFromDate, ref DateTime? searchToDate, string transactionDateFrom = null, string transactionDateTo = null)
        {
            DateTime temp;

            if (!string.IsNullOrEmpty(transactionDateFrom))
            {
                searchFromDate = DateTime.TryParse(transactionDateFrom, out temp) ? temp : null;
            }
            if (!string.IsNullOrEmpty(transactionDateTo))
            {
                searchToDate = DateTime.TryParse(transactionDateTo, out temp) ? temp : null;
            }

            //validate transactionDate search
            if (!string.IsNullOrEmpty(transactionDateFrom) && !string.IsNullOrEmpty(transactionDateTo)
                && (searchFromDate > searchToDate || searchToDate < searchFromDate))
            {
                _logger.LogInformation(LogEvents.GetItem, $"BadRequest Error. TransactionDateFrom should be less than TransactionDateTo.");
                throw new ApiException(HttpStatusCode.BadRequest, TransactionErrorCodes.InvalidTransactionDateSearchPeriod.ToString());
            }
        }

        public async Task<PagedResponse<TransactionModel>> GetTransactionsAsync(int accountId, FilterModel filterModel, string transactionDateFrom = null, string transactionDateTo = null)
        {
            //validate account
            await _validationService.ValidateAccountAsync(accountId);

            DateTime? searchFromDate = null, searchToDate = null;
            ParseSearchTransactionDates(ref searchFromDate, ref searchToDate, transactionDateFrom, transactionDateTo);

            int page = (filterModel.PageIndex - 1) * filterModel.PageSize;
            if (page < 0)
            {
                page = 0;
            }
            var transactions = new List<Transaction>();
            int totalTransactionsCount = 0;

            var sortPredicate = MapSortProperty(filterModel.SortProperty);

            if (!string.IsNullOrEmpty(filterModel.SearchString))
            {
                var searchString = filterModel.SearchString.ToLower();

                var transactionsQuery = _context.Transaction.AsNoTracking()
                     .Where(s => s.AccountId == accountId &&
                     (searchFromDate == null || (searchFromDate != null && s.TransactionDate.Date >= searchFromDate)) &&
                     (searchToDate == null || (searchToDate != null && s.TransactionDate.Date <= searchToDate))
                     && (s.ReferenceNumber.ToLower().Contains(searchString) || s.Description.ToLower().Contains(searchString)
                     || s.Debit.ToString().Contains(searchString) || s.Credit.ToString().Contains(searchString)
                     || s.TranscationType.ToLower().Contains(searchString) || s.BalanceAfter.ToString().Contains(searchString)
                     || s.TransactionDate.Date.ToString().Contains(searchString)))
                     .Sort(sortPredicate, filterModel.SortOrder);

                totalTransactionsCount = await _context.Transaction.CountAsync();
                transactions = await transactionsQuery.Skip(page).Take(filterModel.PageSize).ToListAsync();
            }
            else
            {
                transactions = await _context.Transaction.AsNoTracking()
                                .Sort(sortPredicate, filterModel.SortOrder)
                                .Skip(page).Take(filterModel.PageSize)
                                .Where(s => s.AccountId == accountId &&
                                (searchFromDate == null || (searchFromDate != null && s.TransactionDate.Date >= searchFromDate)) &&
                                (searchToDate == null || (searchToDate != null && s.TransactionDate.Date <= searchToDate)))
                                .ToListAsync();

                totalTransactionsCount = await _context.Transaction.CountAsync();
            }

            if (!transactions.Any())
            {
                _logger.LogInformation(LogEvents.ListItems, "No transactions were found.");
                return PagedResponse<TransactionModel>.Empty();
            }
            else
            {
                var items = _mapper.Map<IEnumerable<TransactionModel>>(transactions);
                return new PagedResponse<TransactionModel>(items, totalTransactionsCount);
            }
        }

        private Expression<Func<Transaction, object>> MapSortProperty(string sortProperty)
        {
            Expression<Func<Transaction, object>> sortExpression = null;
            switch (sortProperty)
            {
                case "id":
                    sortExpression = x => x.Id;
                    break;
                case "referenceNumber":
                    sortExpression = x => x.ReferenceNumber;
                    break;
                case "description":
                    sortExpression = x => x.Description;
                    break;
                case "debit":
                    sortExpression = x => x.Debit;
                    break;
                case "credit":
                    sortExpression = x => x.Credit;
                    break;
                case "transactionDate":
                    sortExpression = x => x.TransactionDate;
                    break;
                case "transcationType":
                    sortExpression = x => x.TranscationType;
                    break;
                case "balanceAfter":
                    sortExpression = x => x.BalanceAfter;
                    break;
                default:
                    sortExpression = x => x.TransactionDate;
                    break;
            }
            return sortExpression;
        }
    }
}