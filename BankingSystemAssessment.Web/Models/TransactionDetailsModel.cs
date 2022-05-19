namespace BankingSystemAssessment.Web.Models
{
    public class TransactionDetailsModel
    {
        public int Id { get; set; }
        public string ReferenceNumber { get; set; }
        public string Description { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public string TransactionDate { get; set; }
        public string TranscationType { get; set; }
        public decimal BalanceAfter { get; set; }
    }
}