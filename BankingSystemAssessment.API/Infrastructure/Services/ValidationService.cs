using BankingSystemAssessment.API.Infrastructure.Domain;
using BankingSystemAssessment.Core.ErrorHandling.Exceptions;
using BankingSystemAssessment.Core.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
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
    }
}