using System.Threading.Tasks;
using BankingSystemAssessment.Web.ApiClients.Contracts;
using BankingSystemAssessment.Web.Filters;
using BankingSystemAssessment.Web.Infrastructure;
using BankingSystemAssessment.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BankingSystemAssessment.Web.Pages
{
    [TypeFilter(typeof(PageExceptionFilter))]
    public class DetailsModel : PageModel
    {
        private readonly ICustomerApiClient customerApiClient;
        protected readonly ICorrelationIdResolver correlationIdResolver;

        public DetailsModel(ICorrelationIdResolver correlationIdResolver, ICustomerApiClient customerApiClient)
        {
            this.correlationIdResolver = correlationIdResolver;
            this.customerApiClient = customerApiClient;
        }

        public CustomerDetailsModel Customer { get; set; }
        
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Customer = await customerApiClient.GetCustomerDetailsAsync(id);
            return Page();
        }
    }
}