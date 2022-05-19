using BankingSystemAssessment.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace BankingSystemAssessment.Web.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public class ErrorModel : PageModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        [BindProperty]
        public ErrorResult ErrorResult { get; set; }

        public string ErrorResultstr { get; set; }

        public void OnGet(string errorResult)
        {
            ErrorResult =  JsonSerializer.Deserialize<ErrorResult>(errorResult);
        }
    }
}
