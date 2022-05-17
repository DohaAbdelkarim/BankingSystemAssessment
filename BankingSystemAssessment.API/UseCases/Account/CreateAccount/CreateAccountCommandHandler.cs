using BankingSystemAssessment.API.Infrastructure.Services;
using BankingSystemAssessment.API.Models;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BankingSystemAssessment.API.UseCases.Account
{
    public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, CreateAccountResponseModel>
    {
        private readonly IAccountService _accountService;

        public CreateAccountCommandHandler(IAccountService accountService)
        {
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
        }

        public async Task<CreateAccountResponseModel> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            var accountResponseModel = await _accountService.CreateAccountAsync(request.CreateAccountRequestModel);
            return accountResponseModel;
        }    
    }
}