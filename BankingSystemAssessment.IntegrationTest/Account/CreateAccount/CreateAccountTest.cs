using BankingSystemAssessment.API;
using BankingSystemAssessment.API.Models;
using BankingSystemAssessment.Core.Extensions.HttpExtensions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BankingSystemAssessment.IntegrationTest.Account
{
    public class CreateAccountTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;
        public CreateAccountTest(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }
        public string createAccountURL = "api/Account";

        [Theory]
        [InlineData(2,5000)]
        public async Task CreateAccount_ValidModel_ReturnAccepted(int customerId, decimal initialCredit)
        {
            // Arrange
            var client = _factory.ConfigureTest().CreateClient();
            var createAccountRequestModel = new CreateAccountRequestModel(customerId, initialCredit);
            var payload = JsonConvert.SerializeObject(createAccountRequestModel);
            var content = new StringContent(payload, encoding: UTF8Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync(createAccountURL, content);

            // Assert
            var createAccountResponseModel = await response.DeserializeResponseAsync<CreateAccountResponseModel>();
            Assert.True(createAccountResponseModel.AccountId > 0);
        }
      
        [Theory]
        [InlineData(500, 0)]
        public async Task CreateAccount_InvalidCustomerId_ReturnNotFound(int customerId, decimal initialCredit)
        {
            // Arrange
            var client = _factory.ConfigureTest().CreateClient();
            var createAccountRequestModel = new CreateAccountRequestModel(customerId, initialCredit);
            var payload = JsonConvert.SerializeObject(createAccountRequestModel);
            var content = new StringContent(payload, encoding: UTF8Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync(createAccountURL, content);

            // Assert
            Assert.Equal(StatusCodes.Status404NotFound, (int)response.StatusCode);
        }

        [Theory]
        [InlineData(0, 0)]
        public async Task CreateAccount_InvalidCustomerId_ReturnBadRequest(int customerId, decimal initialCredit)
        {
            // Arrange
            var client = _factory.ConfigureTest().CreateClient();
            var createAccountRequestModel = new CreateAccountRequestModel(customerId, initialCredit);
            var payload = JsonConvert.SerializeObject(createAccountRequestModel);
            var content = new StringContent(payload, encoding: UTF8Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync(createAccountURL, content);

            // Assert
            Assert.Equal(StatusCodes.Status400BadRequest, (int)response.StatusCode);
        }

        [Theory]
        [InlineData(1, -1)]
        public async Task CreateAccount_InvalidInitialCredit_ReturnBadRequest(int customerId, decimal initialCredit)
        {
            // Arrange
            var client = _factory.ConfigureTest().CreateClient();
            var createAccountRequestModel = new CreateAccountRequestModel(customerId, initialCredit);
            var payload = JsonConvert.SerializeObject(createAccountRequestModel);
            var content = new StringContent(payload, encoding: UTF8Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync(createAccountURL, content);

            // Assert
            Assert.Equal(StatusCodes.Status400BadRequest, (int)response.StatusCode);
        }
    }
}
