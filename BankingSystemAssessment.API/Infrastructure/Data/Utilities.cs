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
            bankingSystemContext.Customer.Add(new Domain.Customer { Id = 1, FirstName = "Doha", LastName = "Abdelkarim", Email = "doha.hamed.mohamed@gmail.com", Mobile = "01022515455", Address = "Nasr city", CustomerID = "1234542612688859" });
            bankingSystemContext.Customer.Add(new Domain.Customer { Id = 2, FirstName = "Peter", LastName = "Adel", Email = "Peter.Adel@gmail.com", Mobile = "01188961227", Address = "New cairo", CustomerID = "1230542612688857" });
        }

        private static void SeedAccountRecords(BankingSystemContext bankingSystemContext)
        {
            bankingSystemContext.Account.Add(new Domain.Account { Id = 1, CustomerId = 1, AccountNumber = "100048648888", Balance = 1000, Currency = Currency.EGP.ToString(), Status = AccountStatus.Active.ToString(), CreatedDate = new DateTime(2021, 05, 16) });
            bankingSystemContext.Account.Add(new Domain.Account { Id = 2, CustomerId = 2, AccountNumber = "100078592250", Balance = 9600, Currency = Currency.EGP.ToString(), Status = AccountStatus.Active.ToString(), CreatedDate = new DateTime(2020, 05, 16) });
            bankingSystemContext.Account.Add(new Domain.Account { Id = 3, CustomerId = 2, AccountNumber = "100078592255", Balance = 5000, Currency = Currency.USD.ToString(), Status = AccountStatus.Active.ToString(), CreatedDate = new DateTime(2022, 05, 16) });
            bankingSystemContext.Account.Add(new Domain.Account { Id = 4, CustomerId = 2, AccountNumber = "100078592255", Balance = 0, Currency = Currency.EGP.ToString(), Status = AccountStatus.Suspended.ToString(), CreatedDate = new DateTime(2022, 05, 16) });
        }

        private static void SeedTransactionRecords(BankingSystemContext bankingSystemContext)
        {
            bankingSystemContext.Transaction.Add(new Domain.Transaction { Id = 1, AccountId = 1, ReferenceNumber = "FT1519991872", Description = "Deposit 1000 EGP", Debit = 0, Credit = 2000, TransactionDate = new DateTimeOffset(2021, 05, 16, 15, 26, 07, new TimeSpan(1, 0, 0)), TranscationType = TranscationType.Deposit.ToString(), BalanceAfter = 2000 });
            bankingSystemContext.Transaction.Add(new Domain.Transaction { Id = 2, AccountId = 1, ReferenceNumber = "FT1529991872", Description = "Withdrawal 1000 EGP", Debit = 1000, Credit = 0, TransactionDate = new DateTimeOffset(2021, 08, 17, 15, 26, 07, new TimeSpan(1, 0, 0)), TranscationType = TranscationType.Withdrawal.ToString(), BalanceAfter = 1000 });

            bankingSystemContext.Transaction.Add(new Domain.Transaction { Id = 3, AccountId = 2, ReferenceNumber = "FT1539991872", Description = "Deposit 10000 EGP", Debit = 0, Credit = 10000, TransactionDate = new DateTimeOffset(2020, 05, 16, 15, 26, 07, new TimeSpan(1, 0, 0)), TranscationType = TranscationType.Deposit.ToString(), BalanceAfter = 10000 });
            bankingSystemContext.Transaction.Add(new Domain.Transaction { Id = 4, AccountId = 2, ReferenceNumber = "FT1514991872", Description = "Withdrawal 1000 EGP", Debit = 1000, Credit = 0, TransactionDate = new DateTimeOffset(2020, 08, 16, 15, 26, 07, new TimeSpan(1, 0, 0)), TranscationType = TranscationType.Withdrawal.ToString(), BalanceAfter = 9000 });
            bankingSystemContext.Transaction.Add(new Domain.Transaction { Id = 5, AccountId = 2, ReferenceNumber = "FT1519591872", Description = "Deposit 600 EGP", Debit = 0, Credit = 600, TransactionDate = new DateTimeOffset(2022, 05, 16, 15, 26, 07, new TimeSpan(1, 0, 0)), TranscationType = TranscationType.Deposit.ToString(), BalanceAfter = 9600 });

            bankingSystemContext.Transaction.Add(new Domain.Transaction { Id = 6, AccountId = 3, ReferenceNumber = "FT1519971872", Description = "Deposit 5000 USD", Debit = 0, Credit = 5000, TransactionDate = new DateTimeOffset(2022, 05, 16, 15, 26, 07, new TimeSpan(1, 0, 0)), TranscationType = TranscationType.Deposit.ToString(), BalanceAfter = 5000 });
        }
    }
}