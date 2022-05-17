using System;

namespace BankingSystemAssessment.Core.Common
{
    public static class UniqueIdGenerator
    {
        public static string GenerateUniqueId(int size)
        {
            Random random = new Random();

            char[] generated = new char[size];
            for (int i = 0; i < size; i++)
            {
                generated[i] = (char)('0' + random.Next(10));
            }

            string uniqueId = string.Join(string.Empty, generated);
            return uniqueId;
        }
    }
}