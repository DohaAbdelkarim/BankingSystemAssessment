using BankingSystemAssessment.Core.Common;

namespace BankingSystemAssessment.API.Infrastructure.Helpers
{
    public static class TransactionReferenceNumberHelper
    {
        /* 
        * Sample TransactionReferenceId: 100012345678
        * 
        * FT       Transaction Reference identifier
        * 1234567891   Random unique number of 10 digits
        */

        //Assume that the Transaction Reference identifier code
        const string UniqueTransactionReferenceCode = Constants.UniqueTransactionReferenceCode;

        public static string GenerateTransactionReferenceNumber()
        {
            string uniqueId = UniqueIdGenerator.GenerateUniqueId(10);
            return $"{UniqueTransactionReferenceCode}{uniqueId}";
        }
    }
}