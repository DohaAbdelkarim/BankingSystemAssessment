namespace BankingSystemAssessment.API.Models
{
    public class CreateAccountRequestModel
    {
        public CreateAccountRequestModel() { }
        public CreateAccountRequestModel(int customerId, decimal initialCredit)
        {
            CustomerId = customerId;
            InitialCredit = initialCredit;
        }

        public int CustomerId { get; set; }
        public decimal InitialCredit { get; set; }
    }
}