﻿using BankingSystemAssessment.API.Infrastructure.Domain;
using System.Threading.Tasks;

namespace BankingSystemAssessment.API.Infrastructure.Services
{
    public interface IValidationService
    {
        Task<Customer> ValidateCustomerAsync(int customerId);
    }
}