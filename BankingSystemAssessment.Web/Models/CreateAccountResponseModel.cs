namespace BankingSystemAssessment.Web.Models
{
    public class CreateAccountResponseModel
    {
        public CreateAccountResponseModel(int accountId)
        {
            AccountId = accountId;
        }
        public int AccountId { get; set; }
    }
}