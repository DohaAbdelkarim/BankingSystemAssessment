namespace BankingSystemAssessment.Web.Models
{
    public class CreateAccountRequestModel
    {
        public CreateAccountRequestModel() { }
        public CreateAccountRequestModel(string customerID, decimal initialCredit)
        {
            CustomerID = customerID;
            InitialCredit = initialCredit;
        }

        public string CustomerID { get; set; }
        public decimal InitialCredit { get; set; }
    }
}