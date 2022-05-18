using BankingSystemAssessment.API.Infrastructure.Services;
using BankingSystemAssessment.API.Models;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BankingSystemAssessment.API.UseCases.Customer
{
    public class CustomerByIdQueryHandler : IRequestHandler<CustomerByIdQuery, CustomerDetailsModel>
    {
        private readonly ICustomerService _customerService;

        public CustomerByIdQueryHandler(ICustomerService customerService)
        {
            _customerService = customerService ?? throw new ArgumentNullException(nameof(customerService));
        }
        public async Task<CustomerDetailsModel> Handle(CustomerByIdQuery request, CancellationToken cancellationToken)
        {
            return await _customerService.GetCustomerDetailsAsync(request.CustomerId, request.TransactionsCount);
        }
    }
}