using BankingSystemAssessment.API.Infrastructure.Domain;
using BankingSystemAssessment.API.Infrastructure.Enums;
using BankingSystemAssessment.Core.ErrorHandling.Exceptions;
using BankingSystemAssessment.Core.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BankingSystemAssessment.API.Infrastructure.Services
{
    public class ValidationService : IValidationService
    {
        private readonly BankingSystemContext _context;
        private readonly ILogger<ValidationService> _logger;

        public ValidationService(BankingSystemContext context, ILogger<ValidationService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Customer> ValidateCustomerAsync(int customerId)
        {
            var customer = await _context.Customer.AsNoTracking().Where(s => s.Id == customerId).FirstOrDefaultAsync();

            if (customer == null)
            {
                _logger.LogInformation(LogEvents.GetItemNotFound, $"Customer with Id {customerId} doesn't exist.");
                throw new NotFoundException();
            }
            return customer;
        }

        public async Task<Account> ValidateAccountAsync(int accountId)
        {
            var account = await _context.Account.AsNoTracking().Where(s => s.Id == accountId).FirstOrDefaultAsync();

            if (account == null)
            {
                _logger.LogInformation(LogEvents.GetItemNotFound, $"Account with Id {accountId} doesn't exist.");
                throw new NotFoundException();
            }
            
            if (account.Status != AccountStatus.Active.ToString())
            {
                _logger.LogInformation(LogEvents.GetItem, $"BadRequest Error. Account with Id:{accountId} isn't active any more.");
                throw new ApiException(HttpStatusCode.BadRequest, AccountErrorCodes.NotActiveAccount.ToString()); //maybe terminated or suspended
            }
            return account;
        }
    }
}