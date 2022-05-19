using System.Collections.Generic;

namespace BankingSystemAssessment.Web.Models
{
    public class AccountDetailsModel
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public string Currency { get; set; }
        public string Status { get; set; }
        public List<TransactionDetailsModel> Transactions { get; set; }
    }
}