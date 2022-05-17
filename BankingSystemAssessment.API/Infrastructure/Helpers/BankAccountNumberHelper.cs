using BankingSystemAssessment.Core.Common;

namespace BankingSystemAssessment.API.Infrastructure.Helpers
{
    public static class BankAccountNumberHelper
    {
        /* 
        * Sample UniqueId: 100012345678
        * 
        * 1000       Unique bank identifier
        * 12345678   Random unique number
        */

        //Assume that the bank initial identifier code=1000
        const string UniqueBankIdentifierCode = Constants.UniqueBankIdentifierCode;

        public static string GenerateAccountUniqueId()
        {
            string uniqueId = UniqueIdGenerator.GenerateUniqueId(8);
            return $"{UniqueBankIdentifierCode}{uniqueId}";
        }
    }
}