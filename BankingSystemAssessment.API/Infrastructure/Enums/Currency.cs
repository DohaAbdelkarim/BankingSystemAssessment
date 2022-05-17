using System.ComponentModel.DataAnnotations;

namespace BankingSystemAssessment.API.Infrastructure.Enums
{
    public enum Currency
    {
        [Display(Name = "Euro")]
        EUR = 1,
        [Display(Name = "United States Dollar")]
        USD = 2,
        [Display(Name = "Egyptain Pound")]
        EGP = 3
    }
}