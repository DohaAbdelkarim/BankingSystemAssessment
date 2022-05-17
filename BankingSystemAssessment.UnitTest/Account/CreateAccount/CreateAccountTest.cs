using BankingSystemAssessment.API.Models;
using BankingSystemAssessment.Core.ErrorHandling.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Xunit;

namespace BankingSystemAssessment.UnitTest.Account
{
    public class CreateAccountTest
    {
        [Theory]
        [InlineData(2, 5000)]
        public async Task CreateAccount_ReturnCreateAccountResponseModel(int customerId, decimal initialCredit)
        {
            //Arrange
            var createAccountRequestModel = new CreateAccountRequestModel(customerId, initialCredit);
            var accountService = TestFactories.AccountServiceTestFactory();

            // Act
            var createAccountResponseModel = await accountService.CreateAccountAsync(createAccountRequestModel);

            // Assert
            Assert.NotNull(createAccountResponseModel);
            Assert.True(createAccountResponseModel.AccountId > 0);
        }

        [Theory]
        [InlineData(500, 0)]
        public async Task CreateAccount_InvalidCustomerId_ReturnNotFound(int customerId, decimal initialCredit)
        {
            // Arrange
            var createAccountRequestModel = new CreateAccountRequestModel(customerId, initialCredit);
            var accountService = TestFactories.AccountServiceTestFactory();

            // Act
            Task createAccount() => accountService.CreateAccountAsync(createAccountRequestModel);

            // Assert
            var notFoundException = await Assert.ThrowsAsync<NotFoundException>(createAccount);
            Assert.Equal(StatusCodes.Status404NotFound, (int)notFoundException.StatusCode);
        }
    }
}