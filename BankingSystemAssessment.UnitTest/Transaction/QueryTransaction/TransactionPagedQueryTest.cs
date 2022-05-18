using BankingSystemAssessment.Core.ErrorHandling.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using BankingSystemAssessment.Core.Common.Models;
using System.Linq;

namespace BankingSystemAssessment.UnitTest.Transaction
{
    public class TransactionPagedQueryTest
    {
        public static IEnumerable<object[]> Account_ValidModel()
        {
            yield return new object[] { 2, new FilterModel { PageIndex = 1, PageSize = 10, SearchString= "Deposit" }, null, null };
        }
        [Theory]
        [MemberData(nameof(Account_ValidModel))]
        public async Task GetTransactions_ReturnPagedTransactionModel(int accountId, FilterModel filterModel, string transactionDateFrom, string transactionDateTo)
        {
            //Arrange
            var transactionService = TestFactories.TransactionServiceTestFactory();

            // Act
            var pagedTransactionModel = await transactionService.GetTransactionsAsync(accountId, filterModel, transactionDateFrom, transactionDateTo);

            // Assert
            Assert.NotNull(pagedTransactionModel);
            Assert.True(pagedTransactionModel.Total > 0);
            Assert.True(pagedTransactionModel.Data.Count()==2);
            pagedTransactionModel.Data.ToList().ForEach(x =>
            {
                Assert.Contains(filterModel.SearchString.ToLower(), x.TranscationType.ToLower());
            });
        }

        public static IEnumerable<object[]> Account_TransactionDateSearch_ValidModel()
        {
            yield return new object[] { 2, new FilterModel { PageIndex = 1, PageSize = 10}, "2020-04-16", "2020-05-18" };
        }
        [Theory]
        [MemberData(nameof(Account_TransactionDateSearch_ValidModel))]
        public async Task GetTransactions_ReturnFilteredPagedTransactionModel(int accountId, FilterModel filterModel, string transactionDateFrom, string transactionDateTo)
        {
            //Arrange
            var transactionService = TestFactories.TransactionServiceTestFactory();

            // Act
            var pagedTransactionModel = await transactionService.GetTransactionsAsync(accountId, filterModel, transactionDateFrom, transactionDateTo);

            // Assert
            Assert.NotNull(pagedTransactionModel);
            Assert.True(pagedTransactionModel.Total > 0);
            Assert.True(pagedTransactionModel.Data.Count() == 1);
        }

        public static IEnumerable<object[]> Account_InValidAccountId()
        {
            yield return new object[] { 400, new FilterModel { PageIndex = 1, PageSize = 10 }, null, null };
        }
        [Theory]
        [MemberData(nameof(Account_InValidAccountId))]
        public async Task GetTransactions_ReturnNotFound(int accountId, FilterModel filterModel, string transactionDateFrom, string transactionDateTo)
        {
            //Arrange
            var transactionService = TestFactories.TransactionServiceTestFactory();

            // Act
            Task getTransactions() => transactionService.GetTransactionsAsync(accountId, filterModel, transactionDateFrom, transactionDateTo);

            // Assert
            var notFoundException = await Assert.ThrowsAsync<NotFoundException>(getTransactions);
            Assert.Equal(StatusCodes.Status404NotFound, (int)notFoundException.StatusCode);
        }

        public static IEnumerable<object[]> Account_InValidAccountStatus()
        {
            yield return new object[] { 4, new FilterModel { PageIndex = 1, PageSize = 10 }, null, null };
        }
        [Theory]
        [MemberData(nameof(Account_InValidAccountStatus))]
        public async Task GetTransactions_ReturnBadRequest(int accountId, FilterModel filterModel, string transactionDateFrom, string transactionDateTo)
        {
            //Arrange
            var transactionService = TestFactories.TransactionServiceTestFactory();

            // Act
            Task getTransactions() => transactionService.GetTransactionsAsync(accountId, filterModel, transactionDateFrom, transactionDateTo);
           
            // Assert
            var badRequestException = await Assert.ThrowsAsync<ApiException>(getTransactions);
            Assert.Equal(StatusCodes.Status400BadRequest, (int)badRequestException.StatusCode);
        }
    }
}
