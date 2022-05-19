using BankingSystemAssessment.Core.Common.Models;
using BankingSystemAssessment.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingSystemAssessment.Web.ApiClients.Contracts
{
    public interface ICustomerApiClient
    {
        Task<PagedResponse<CustomerIndexModel>> GetCustomersAsync();
        Task<CustomerDetailsModel> GetCustomerDetailsAsync(int customerId);
    }
}
