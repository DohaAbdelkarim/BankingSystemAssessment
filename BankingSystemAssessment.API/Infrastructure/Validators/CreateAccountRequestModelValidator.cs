using BankingSystemAssessment.API.Infrastructure.Enums;
using BankingSystemAssessment.API.Models;
using FluentValidation;

namespace BankingSystemAssessment.API.Infrastructure.Validators
{
    public class CreateAccountRequestModelValidator : AbstractValidator<CreateAccountRequestModel>
    {
        public CreateAccountRequestModelValidator()
        {
            RuleFor(x => x.CustomerId).NotNull().WithMessage(AccountErrorCodes.CustomerIdRequired.ToString());
            RuleFor(x => x.CustomerId).GreaterThan(0).WithMessage(AccountErrorCodes.InvalidCustomerId.ToString());
            RuleFor(x => x.InitialCredit).GreaterThanOrEqualTo(0).WithMessage(AccountErrorCodes.InvalidInitialCredit.ToString());
        }
    }
}