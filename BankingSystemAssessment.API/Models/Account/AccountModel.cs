using System.Collections.Generic;

namespace BankingSystemAssessment.API.Models
{
    public class AccountModel
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public string Currency { get; set; }
        public string Status { get; set; }
        public List<TransactionModel> Transactions { get; set; }
    }
}