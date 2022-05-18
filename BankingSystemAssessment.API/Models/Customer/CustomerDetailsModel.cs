using System.Collections.Generic;

namespace BankingSystemAssessment.API.Models
{
    public class CustomerDetailsModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<AccountModel> Accounts { get; set; }
    }
}