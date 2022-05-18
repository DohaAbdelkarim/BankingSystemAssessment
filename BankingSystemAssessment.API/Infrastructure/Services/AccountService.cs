using AutoMapper;
using BankingSystemAssessment.API.Infrastructure.Domain;
using BankingSystemAssessment.API.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using BankingSystemAssessment.API.Infrastructure.Helpers;
using BankingSystemAssessment.Core.Extensions.DatabaseExtensions;

namespace BankingSystemAssessment.API.Infrastructure.Services
{
    public class AccountService : IAccountService
    {
        private readonly BankingSystemContext _context;
        private readonly ILogger<AccountService> _logger;
        private readonly IMapper _mapper;
        private readonly IValidationService _validationService;
        private readonly ITransactionService _transactionService;

        public AccountService(BankingSystemContext context, ILogger<AccountService> logger, IMapper mapper, IValidationService validationService, ITransactionService transactionService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _validationService = validationService ?? throw new ArgumentNullException(nameof(validationService));
            _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
        }

        public async Task<CreateAccountResponseModel> CreateAccountAsync(CreateAccountRequestModel createAccountRequestModel)
        {
            var customer = await _validationService.ValidateCustomerAsync(createAccountRequestModel.CustomerId);

            //Generate account number and make sure it's unique
            string generatedAccountNumber = string.Empty;
            do
            {
                generatedAccountNumber = BankAccountNumberHelper.GenerateAccountUniqueId();
            } while (_context.Account.Any(a => a.AccountNumber.Equals(generatedAccountNumber)));

            Account account = _mapper.Map<Account>(createAccountRequestModel);
            account.AccountNumber = generatedAccountNumber;

            if (createAccountRequestModel.InitialCredit > 0)
            {
                var deposit = await _transactionService.CreateDepositAsync(account, createAccountRequestModel.InitialCredit, true);
            }
            else
            {
                await _context.PostAsync(account, _logger);
            }
            _logger.LogInformation($"An Account {account.AccountNumber} is created for customer Id: {customer.Id}.");
            return new CreateAccountResponseModel(account.Id);
        }
    }
}