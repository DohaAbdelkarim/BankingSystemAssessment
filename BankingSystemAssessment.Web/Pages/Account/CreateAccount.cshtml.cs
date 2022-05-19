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
    public class CreateAccountModel : PageModel
    {
        private readonly IAccountApiClient accountApiClient;
        protected readonly ICorrelationIdResolver correlationIdResolver;

        public CreateAccountModel(ICorrelationIdResolver correlationIdResolver, IAccountApiClient accountApiClient)
        {
            this.correlationIdResolver = correlationIdResolver;
            this.accountApiClient = accountApiClient;
        }

        [BindProperty]
        public CreateAccountRequestModel createAccountRequestModel { get; set; }

        public void OnGet()
        {
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync()
        {
            await accountApiClient.CreateAccountAsync(createAccountRequestModel);
            return RedirectToPage("/Customer/Index");
        }
    }
}