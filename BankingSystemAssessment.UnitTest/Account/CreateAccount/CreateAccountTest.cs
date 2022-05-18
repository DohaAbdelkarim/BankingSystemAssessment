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
        [InlineData("1230542612688857", 5000)]
        public async Task CreateAccount_ReturnCreateAccountResponseModel(string customerID, decimal initialCredit)
        {
            //Arrange
            var createAccountRequestModel = new CreateAccountRequestModel(customerID, initialCredit);
            var accountService = TestFactories.AccountServiceTestFactory();

            // Act
            var createAccountResponseModel = await accountService.CreateAccountAsync(createAccountRequestModel);

            // Assert
            Assert.NotNull(createAccountResponseModel);
            Assert.True(createAccountResponseModel.AccountId > 0);
        }

        [Theory]
        [InlineData("1234542612688851", 0)]
        public async Task CreateAccount_InvalidCustomerId_ReturnNotFound(string customerID, decimal initialCredit)
        {
            // Arrange
            var createAccountRequestModel = new CreateAccountRequestModel(customerID, initialCredit);
            var accountService = TestFactories.AccountServiceTestFactory();

            // Act
            Task createAccount() => accountService.CreateAccountAsync(createAccountRequestModel);

            // Assert
            var notFoundException = await Assert.ThrowsAsync<NotFoundException>(createAccount);
            Assert.Equal(StatusCodes.Status404NotFound, (int)notFoundException.StatusCode);
        }
    }
}