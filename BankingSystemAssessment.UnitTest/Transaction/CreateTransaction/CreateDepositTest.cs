using BankingSystemAssessment.Core.ErrorHandling.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using BankingSystemAssessment.API.Infrastructure.Enums;
using System;

namespace BankingSystemAssessment.UnitTest.Transaction
{
    public class CreateDepositTest
    {
        public static IEnumerable<object[]> Account_ValidModel()
        {
            yield return new object[] { new API.Infrastructure.Domain.Account { Id = 0,
                CustomerId = 2, AccountNumber = "100078592285", Balance = 0,
                Currency = Currency.EGP.ToString(),
                Status = AccountStatus.Active.ToString(),
                CreatedDate = new DateTime(2021, 05, 16) } ,5000,true };
        }
        [Theory]
        [MemberData(nameof(Account_ValidModel))]
        public async Task CreateDeposit_ReturnTransaction(API.Infrastructure.Domain.Account account, decimal credit, bool initialCredit)
        {
            //Arrange
            var transactionService = TestFactories.TransactionServiceTestFactory();

            // Act
            var deposit = await transactionService.CreateDepositAsync(account, credit, initialCredit);

            // Assert
            Assert.NotNull(deposit);
            Assert.True(deposit.Id > 0);
            Assert.Equal(credit, deposit.Credit);
        }

        public static IEnumerable<object[]> Account_InValidModel()
        {
            yield return new object[] { new API.Infrastructure.Domain.Account { Id = 10,
                CustomerId = 2, AccountNumber = "100078592285", Balance = 0,
                Currency = Currency.EGP.ToString(),
                Status = AccountStatus.Active.ToString(),
                CreatedDate = new DateTime(2021, 05, 16) } , 5000, false };
        }

        [Theory]
        [MemberData(nameof(Account_InValidModel))]
        public async Task CreateDeposit_ReturnNotFound(API.Infrastructure.Domain.Account account, decimal credit, bool initialCredit)
        {
            //Arrange
            var transactionService = TestFactories.TransactionServiceTestFactory();

            // Act
            Task createDeposit() => transactionService.CreateDepositAsync(account, credit, initialCredit);

            // Assert
            var notFoundException = await Assert.ThrowsAsync<NotFoundException>(createDeposit);
            Assert.Equal(StatusCodes.Status404NotFound, (int)notFoundException.StatusCode);
        }

        public static IEnumerable<object[]> Account_SuspendedModel()
        {
            yield return new object[] { new API.Infrastructure.Domain.Account { Id = 0,
                CustomerId = 2, AccountNumber = "100078592255", Balance = 0,
                Currency = Currency.EGP.ToString(),
                Status = AccountStatus.Suspended.ToString(),
                CreatedDate = new DateTime(2021, 05, 16) },5000,true };
        }
        [Theory]
        [MemberData(nameof(Account_SuspendedModel))]
        public async Task CreateDeposit_InvalidAccount_ReturnBadRequest(API.Infrastructure.Domain.Account account, decimal credit, bool initialCredit)
        {
            // Arrange
            var transactionService = TestFactories.TransactionServiceTestFactory();

            // Act
            Task createDeposit() => transactionService.CreateDepositAsync(account, credit, initialCredit);

            // Assert
            var badRequestException = await Assert.ThrowsAsync<ApiException>(createDeposit);
            Assert.Equal(StatusCodes.Status400BadRequest, (int)badRequestException.StatusCode);
        }
    }
}