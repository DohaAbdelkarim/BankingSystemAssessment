using BankingSystemAssessment.API.Infrastructure.Services;
using BankingSystemAssessment.API.Models;
using BankingSystemAssessment.Core.Common.Models;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BankingSystemAssessment.API.UseCases.Customer.QueryCustomer
{
    public class CustomerPagedQueryHandler : IRequestHandler<CustomerPagedQuery, PagedResponse<CustomerIndexModel>>
    {
        private readonly ICustomerService _customerService;

        public CustomerPagedQueryHandler(ICustomerService customerService)
        {
            _customerService = customerService ?? throw new ArgumentNullException(nameof(customerService));
        }
        public async Task<PagedResponse<CustomerIndexModel>> Handle(CustomerPagedQuery request, CancellationToken cancellationToken)
        {
            return await _customerService.GetPagedCustomersAsync(request.FilterModel);
        }
    }
}