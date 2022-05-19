using System.Collections.Generic;

namespace BankingSystemAssessment.Web.Models
{
    public class CustomerDetailsModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<AccountDetailsModel> Accounts { get; set; }
    }
}