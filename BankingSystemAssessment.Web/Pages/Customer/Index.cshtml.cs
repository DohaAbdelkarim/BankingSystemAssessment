using BankingSystemAssessment.Core.Common.Models;
using BankingSystemAssessment.Web.ApiClients.Contracts;
using BankingSystemAssessment.Web.Filters;
using BankingSystemAssessment.Web.Infrastructure;
using BankingSystemAssessment.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace BankingSystemAssessment.Web.Pages
{
    [TypeFilter(typeof(PageExceptionFilter))]
    public class IndexModel : PageModel
    {
        private readonly ICustomerApiClient customerApiClient;
        protected readonly ICorrelationIdResolver correlationIdResolver;

        public IndexModel(ICorrelationIdResolver correlationIdResolver, ICustomerApiClient customerApiClient)
        {
            this.correlationIdResolver = correlationIdResolver;
            this.customerApiClient = customerApiClient;
        }

        public PagedResponse<CustomerIndexModel> Customers { get; set; }

        public async Task OnGetAsync()
        {
            Customers = await customerApiClient.GetCustomersAsync();
        }
    }
}