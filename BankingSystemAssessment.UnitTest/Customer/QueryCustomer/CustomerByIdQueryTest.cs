using BankingSystemAssessment.Core.ErrorHandling.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Xunit;

namespace BankingSystemAssessment.UnitTest.Customer
{
    public class CustomerByIdQueryTest
    {
        [Theory]
        [InlineData(2, 10)]
        public async Task GetCustomerDetails_ReturnCustomerDetailsModel(int customerId, int transactionsCount)
        {
            //Arrange
            var customerService = TestFactories.CustomerServiceTestFactory();

            //Act
            var customerDetailsModel = await customerService.GetCustomerDetailsAsync(customerId, transactionsCount);

            //Assert
            Assert.NotNull(customerDetailsModel);
            Assert.Equal(customerId, customerDetailsModel.Id);
            Assert.NotEmpty(customerDetailsModel.Accounts);
            Assert.True(customerDetailsModel.Accounts.Count == 2);
        }

        [Theory]
        [InlineData(2, 1)]
        public async Task GetCustomerDetails_ReturnPickupDeliveryPointsSorted(int customerId, int transactionsCount)
        {
            //Arrange
            var customerService = TestFactories.CustomerServiceTestFactory();

            //Act
            var customerDetailsModel = await customerService.GetCustomerDetailsAsync(customerId, transactionsCount);

            //Assert
            Assert.NotNull(customerDetailsModel);
            Assert.Equal(customerId, customerDetailsModel.Id);
            Assert.NotEmpty(customerDetailsModel.Accounts);
            Assert.True(customerDetailsModel.Accounts.Count == 2);
            customerDetailsModel.Accounts.ForEach(x =>
            {
                Assert.Equal(transactionsCount, x.Transactions.Count);
            });
        }

        [Theory]
        [InlineData(5000, 10)]
        public async Task GetCustomerDetails_InvalidCustomerId_ThrowNotFoundException(int customerId, int transactionsCount)
        {
            //Arrange
            var customerService = TestFactories.CustomerServiceTestFactory();

            //Act
            Task getCustomerDetails() => customerService.GetCustomerDetailsAsync(customerId, transactionsCount);

            //Assert
            var notFoundException = await Assert.ThrowsAsync<NotFoundException>(getCustomerDetails);
            Assert.Equal(StatusCodes.Status404NotFound, (int)notFoundException.StatusCode);
        }
    }
}