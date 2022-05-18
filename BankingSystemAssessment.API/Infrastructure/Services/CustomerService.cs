using AutoMapper;
using BankingSystemAssessment.API.Infrastructure.Domain;
using BankingSystemAssessment.API.Infrastructure.Enums;
using BankingSystemAssessment.API.Models;
using BankingSystemAssessment.Core.Common.Models;
using BankingSystemAssessment.Core.ErrorHandling.Exceptions;
using BankingSystemAssessment.Core.Extensions.EnumerableExtensions;
using BankingSystemAssessment.Core.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public async Task<PagedResponse<CustomerIndexModel>> GetPagedCustomersAsync(FilterModel filterModel)
        {
            int page = (filterModel.PageIndex - 1) * filterModel.PageSize;
            if (page < 0)
            {
                page = 0;
            }
            var customers = new List<Customer>();
            int totalCustomersCount = 0;

            var sortPredicate = MapSortProperty(filterModel.SortProperty);
            if (!string.IsNullOrEmpty(filterModel.SearchString))
            {
                var searchString = filterModel.SearchString.ToLower();

                var customersQuery = _context.Customer.AsNoTracking()
                    .Where(s => s.Id.Equals(searchString)
                             || searchString.Contains(s.FirstName.ToLower()) || searchString.Contains(s.LastName.ToLower()))
                    .Sort(sortPredicate, filterModel.SortOrder);

                totalCustomersCount = await customersQuery.CountAsync();
                customers = await customersQuery.Skip(page).Take(filterModel.PageSize).ToListAsync();
            }
            else
            {
                customers = await _context.Customer.AsNoTracking()
                     .Sort(sortPredicate, filterModel.SortOrder)
                     .Skip(page).Take(filterModel.PageSize)
                     .ToListAsync();

                totalCustomersCount = await _context.Customer.CountAsync();
            }

            if (!customers.Any())
            {
                _logger.LogInformation(LogEvents.ListItems, "No customers were Found");
                return PagedResponse<CustomerIndexModel>.Empty();
            }
            else
            {
                var items = _mapper.Map<IEnumerable<CustomerIndexModel>>(customers);
                return new PagedResponse<CustomerIndexModel>(items, totalCustomersCount);
            }
        }

        private Expression<Func<Customer, object>> MapSortProperty(string sortProperty)
        {
            Expression<Func<Customer, object>> sortExpression = null;
            switch (sortProperty)
            {
                case "id":
                    sortExpression = x => x.Id;
                    break;
                case "customerName":
                    sortExpression = x => x.FirstName;
                    break;
                default:
                    sortExpression = x => x.Id;
                    break;
            }
            return sortExpression;
        }
    }
}