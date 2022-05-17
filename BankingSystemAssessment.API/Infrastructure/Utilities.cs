using BankingSystemAssessment.API.Infrastructure.Enums;
using System;

namespace BankingSystemAssessment.API.Infrastructure
{
    public static class Utilities
    {
        public static void SeedBankingSystemDb(BankingSystemContext bankingSystemContex)
        {
            SeedCustomerRecords(bankingSystemContex);
            SeedAccountRecords(bankingSystemContex);
            SeedTransactionRecords(bankingSystemContex);
            bankingSystemContex.SaveChanges();
        }

        private static void SeedCustomerRecords(BankingSystemContext bankingSystemContext)
        {
            bankingSystemContext.Customer.Add(new Domain.Customer { Id = 1, FirstName = "Doha", LastName = "Abdelkarim", Email = "doha.hamed.mohamed@gmail.com", Mobile = "01022515455", Address = "Nasr city" });
            bankingSystemContext.Customer.Add(new Domain.Customer { Id = 2, FirstName = "Doha", LastName = "Abdelkarim", Email = "doha.hamed.mohamed@gmail.com", Mobile = "01022515455", Address = "Nasr city" });
        }

        private static void SeedAccountRecords(BankingSystemContext bankingSystemContext)
        {
            bankingSystemContext.Account.Add(new Domain.Account { Id = 1, CustomerId = 1, AccountNumber = "100048648888", Balance = 1000, Currency = Currency.EGP.ToString(), Status = AccountStatus.Active.ToString(), CreatedDate = new DateTime(2022, 05, 16) });
            bankingSystemContext.Account.Add(new Domain.Account { Id = 2, CustomerId = 2, AccountNumber = "100078592250", Balance = 0, Currency = Currency.EGP.ToString(), Status = AccountStatus.Active.ToString(), CreatedDate = new DateTime(2022, 05, 16) });
            bankingSystemContext.Account.Add(new Domain.Account { Id = 3, CustomerId = 2, AccountNumber = "100078592255", Balance = 0, Currency = Currency.EGP.ToString(), Status = AccountStatus.Suspended.ToString(), CreatedDate = new DateTime(2021, 05, 16) });
        }

        private static void SeedTransactionRecords(BankingSystemContext bankingSystemContext)
        {
            bankingSystemContext.Transaction.Add(new Domain.Transaction { Id = 1, AccountId = 1, ReferenceNumber = "FT1519991872", Description = "Deposit 1000 EGP", Debit = 0, Credit = 1000, TransactionDate = new DateTimeOffset(2022, 05, 16, 15, 26, 07, new TimeSpan(1, 0, 0)), TranscationType = TranscationType.Deposit.ToString(), BalanceAfter = 1000 });
        }
    }
}