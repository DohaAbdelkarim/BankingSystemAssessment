using AutoMapper;
using BankingSystemAssessment.API;
using BankingSystemAssessment.API.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System.Reflection;

namespace BankingSystemAssessment.UnitTest
{
    public static class TestFactories
    {
        public static AccountService AccountServiceTestFactory()
        {
            var context = BankingSystemContextMock.GetDBContext();
            var loggerMock = new Mock<ILogger<AccountService>>();
            var mapperMock = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(Assembly.GetEntryAssembly(), typeof(Startup).Assembly);
            });
            var mapper = mapperMock.CreateMapper();

            return new AccountService(context, loggerMock.Object, mapper, ValidationServiceTestFactory(), TransactionServiceTestFactory());
        }

        public static ValidationService ValidationServiceTestFactory()
        {
            var context = BankingSystemContextMock.GetDBContext();
            var loggerMock = new Mock<ILogger<ValidationService>>();
            return new ValidationService(context, loggerMock.Object);
        }

        public static TransactionService TransactionServiceTestFactory()
        {
            var context = BankingSystemContextMock.GetDBContext();
            var loggerMock = new Mock<ILogger<TransactionService>>();
            return new TransactionService(context, loggerMock.Object);
        }
    }
}