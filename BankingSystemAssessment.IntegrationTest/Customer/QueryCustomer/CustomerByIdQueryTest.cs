using BankingSystemAssessment.API;
using BankingSystemAssessment.API.Models;
using BankingSystemAssessment.Core.Extensions.HttpExtensions;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Xunit;

namespace BankingSystemAssessment.IntegrationTest.Customer.QueryCustomer
{
    public class CustomerByIdQueryTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;
        public CustomerByIdQueryTest(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }
        public string GetCustomerDetailsURL = "api/Customer";

        [Theory]
        [InlineData(6000)]
        public async Task GetCustomerDetails_InvalidCustomerId_ReturnNotFound(int id)
        {
            //Arrange
            var url = $"{GetCustomerDetailsURL}/{id}";
            var client = _factory.ConfigureTest().CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.Equal(StatusCodes.Status404NotFound, (int)response.StatusCode);
        }

        [Theory]
        [InlineData(-5)]
        public async Task GetCustomerDetails_InvalidCustomerId_ReturnBadRequest(int id)
        {
            //Arrange
            var url = $"{GetCustomerDetailsURL}/{id}";
            var client = _factory.ConfigureTest().CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.Equal(StatusCodes.Status400BadRequest, (int)response.StatusCode);
        }

        [Theory]
        [InlineData(1,-5)]
        public async Task GetCustomerDetails_InvalidTransactionsCount_ReturnBadRequest(int id, int transactionsCount)
        {
            //Arrange
            var url = $"{GetCustomerDetailsURL}/{id}?transactionsCount={transactionsCount}";
            var client = _factory.ConfigureTest().CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.Equal(StatusCodes.Status400BadRequest, (int)response.StatusCode);
        }

        [Theory]
        [InlineData(2)]
        public async Task GetCustomerDetails_ValidId_VerifyData(int id)
        {
            // Arrange
            var url = $"{GetCustomerDetailsURL}/{id}";
            var client = _factory.ConfigureTest().CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            var customerDetailsModel = await response.DeserializeResponseAsync<CustomerDetailsModel>();
            Assert.NotNull(customerDetailsModel);
            Assert.Equal(id, customerDetailsModel.Id);
            Assert.Equal(2, customerDetailsModel.Accounts.Count);
        }
    }
}