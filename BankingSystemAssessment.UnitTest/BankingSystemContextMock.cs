using BankingSystemAssessment.API.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;

namespace BankingSystemAssessment.UnitTest
{
    public static class BankingSystemContextMock
    {
        public static BankingSystemContext GetDBContext()
        {
            var options = new DbContextOptionsBuilder<BankingSystemContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

            var bankingSystemContext = new BankingSystemContext(options);
            bankingSystemContext.Database.EnsureCreated();
            Utilities.SeedBankingSystemDb(bankingSystemContext);

            //detach all records
            foreach (var dbEntityEntry in bankingSystemContext.ChangeTracker.Entries())
            {
                if (dbEntityEntry.Entity != null)
                {
                    dbEntityEntry.State = EntityState.Detached;
                }
            }
            return bankingSystemContext;
        }
    }
}