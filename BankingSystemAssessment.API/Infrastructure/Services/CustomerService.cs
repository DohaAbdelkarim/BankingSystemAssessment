using AutoMapper;
using BankingSystemAssessment.API.Infrastructure.Enums;
using BankingSystemAssessment.API.Models;
using BankingSystemAssessment.Core.ErrorHandling.Exceptions;
using BankingSystemAssessment.Core.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BankingSystemAssessment.API.Infrastructure.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly BankingSystemContext _context;
        private readonly ILogger<CustomerService> _logger;
        private readonly IMapper _mapper;

        public CustomerService(BankingSystemContext context, ILogger<CustomerService> logger, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<CustomerDetailsModel> GetCustomerDetailsAsync(int customerId, int transactionsCount)
        {
            var customer = await _context.Customer.AsNoTracking()
                .Include(s => s.Account.Where(x => x.Status == AccountStatus.Active.ToString()))
                .ThenInclude(s => s.Transaction.Take(transactionsCount).OrderByDescending(s => s.TransactionDate))
                .Where(s => s.Id == customerId).FirstOrDefaultAsync();

            if (customer == null)
            {
                _logger.LogInformation(LogEvents.GetItemNotFound, $"Customer with Id {customerId} doesn't exist.");
                throw new NotFoundException();
            }
            var customerDetailsModel = _mapper.Map<CustomerDetailsModel>(customer);
            return customerDetailsModel;
        }
    }
}