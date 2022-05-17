using BankingSystemAssessment.API.Models;
using MediatR;

namespace BankingSystemAssessment.API.UseCases.Account
{
    public class CreateAccountCommand : IRequest<CreateAccountResponseModel>
    {
        public CreateAccountRequestModel CreateAccountRequestModel { get; }

        public CreateAccountCommand(CreateAccountRequestModel createAccountRequestModel)
        {
            CreateAccountRequestModel = createAccountRequestModel;
        }
    }
}