using BankingSystemAssessment.API.Models;
using MediatR;

namespace BankingSystemAssessment.API.UseCases.Customer
{
    public class CustomerByIdQuery : IRequest<CustomerDetailsModel>
    {
        public int CustomerId { get; }
        public int TransactionsCount { get; }

        public CustomerByIdQuery(int customerId, int transactionsCount)
        {
            CustomerId = customerId;
            TransactionsCount = transactionsCount;
        }
    }
}