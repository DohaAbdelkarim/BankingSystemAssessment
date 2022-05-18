using BankingSystemAssessment.API;
using BankingSystemAssessment.API.Models;
using BankingSystemAssessment.Core.Common.Models;
using BankingSystemAssessment.Core.Extensions.HttpExtensions;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BankingSystemAssessment.IntegrationTest.Transaction
{
    public class TransactionPagedQueryTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;
        public TransactionPagedQueryTest(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }
        public string GetTransactionsURL = "api/Transaction";

        [Theory]
        [InlineData(400)]
        public async Task GetTransactions_InvalidAccount_ReturnNotFound(int accountId)
        {
            //Arrange
            var url = $"{GetTransactionsURL}/{accountId}";
            var client = _factory.ConfigureTest().CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.Equal(StatusCodes.Status404NotFound, (int)response.StatusCode);
        }

        [Theory]
        [InlineData(4)]
        public async Task GetTransactions_InvalidAccount_ReturnBadRequest(int accountId)
        {
            //Arrange
            var url = $"{GetTransactionsURL}/{accountId}";
            var client = _factory.ConfigureTest().CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.Equal(StatusCodes.Status400BadRequest, (int)response.StatusCode);
        }

        [Theory]
        [InlineData(2, 1, 10, "Deposit")]
        public async Task GetTransactions_Verify_Search(int accountId, int pageIndex, int pageSize, string searchString)
        {
            // Arrange
            var url = $"{GetTransactionsURL}/{accountId}?pageIndex={pageIndex}&pageSize={pageSize}&searchString={searchString}";
            var client = _factory.ConfigureTest().CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            var pagedTransactionModel = await response.DeserializeResponseAsync<PagedResponse<TransactionModel>>();
            Assert.True(pagedTransactionModel.Total > 0);
            Assert.True(pagedTransactionModel.Data.Count() == 2);
            pagedTransactionModel.Data.ToList().ForEach(x =>
            {
                Assert.Contains(searchString.ToLower(), x.TranscationType.ToLower());
            });
        }
        [Theory]
        [InlineData(2, 1, 10, "2020-04-16")]
        public async Task GetTransactions_Verify_SearchTransactionDateFrom(int accountId, int pageIndex, int pageSize, string transactionDateFrom)
        {
            // Arrange
            var url = $"{GetTransactionsURL}/{accountId}?pageIndex={pageIndex}&pageSize={pageSize}&transactionDateFrom={transactionDateFrom}";
            var client = _factory.ConfigureTest().CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            var pagedTransactionModel = await response.DeserializeResponseAsync<PagedResponse<TransactionModel>>();
            Assert.True(pagedTransactionModel.Total > 0);
            Assert.True(pagedTransactionModel.Data.Count() == 3);
        }

        [Theory]
        [InlineData(2, 1, 10, "2020-04-16","2020-05-18")]
        public async Task GetTransactions_Verify_SearchTransactionDate(int accountId, int pageIndex, int pageSize, string transactionDateFrom, string transactionDateTo)
        {
            // Arrange
            var url = $"{GetTransactionsURL}/{accountId}?pageIndex={pageIndex}&pageSize={pageSize}&transactionDateFrom={transactionDateFrom}&transactionDateTo={transactionDateTo}";
            var client = _factory.ConfigureTest().CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            var pagedTransactionModel = await response.DeserializeResponseAsync<PagedResponse<TransactionModel>>();
            Assert.True(pagedTransactionModel.Total > 0);
            Assert.True(pagedTransactionModel.Data.Count() == 1);
        }

        [Theory]
        [InlineData(2, 1, 10, "2022-04-16", "2022-05-18", "Withdrawal")]
        public async Task GetTransactions_Verify_SearchTransactionDateAndTransactionType(int accountId, int pageIndex, int pageSize, string transactionDateFrom, string transactionDateTo, string searchString)
        {
            // Arrange
            var url = $"{GetTransactionsURL}/{accountId}?pageIndex={pageIndex}&pageSize={pageSize}&transactionDateFrom={transactionDateFrom}&transactionDateTo={transactionDateTo}&searchString={searchString}";
            var client = _factory.ConfigureTest().CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            var pagedTransactionModel = await response.DeserializeResponseAsync<PagedResponse<TransactionModel>>();
            Assert.True(pagedTransactionModel.Total == 0);
        }

        [Theory]
        [InlineData(1, 1, 5)]
        public async Task GetTransactions_Verify_Sort_DefaultSortByTransactionDateDescendingly(int accountId, int pageIndex, int pageSize)
        {
            // Arrange
            var url = $"{GetTransactionsURL}/{accountId}?pageIndex={pageIndex}&pageSize={pageSize}";
            var client = _factory.ConfigureTest().CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            var pagedTransactionModel = await response.DeserializeResponseAsync<PagedResponse<TransactionModel>>();
            Assert.True(pagedTransactionModel.Total > 0);
            Assert.Equal(2, pagedTransactionModel.Data.First().Id);
            Assert.Equal(1, pagedTransactionModel.Data.Last().Id);
        }

        [Theory]
        [InlineData(2, 1, 5, "asc", "id")]
        public async Task GetCompanies_Verify_SortAscendingly(int accountId, int pageIndex, int pageSize, string sortOrder, string sortProperty)
        {
            // Arrange
            var url = $"{GetTransactionsURL}/{accountId}?pageIndex={pageIndex}&pageSize={pageSize}&sortOrder={sortOrder}&sortProperty={sortProperty}";
            var client = _factory.ConfigureTest().CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            var pagedTransactionModel = await response.DeserializeResponseAsync<PagedResponse<TransactionModel>>();
            Assert.True(pagedTransactionModel.Total > 0);
            Assert.Equal(3, pagedTransactionModel.Data.First().Id);
            Assert.Equal(5, pagedTransactionModel.Data.Last().Id);
        }

    }
}