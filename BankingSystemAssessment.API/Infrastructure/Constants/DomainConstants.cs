namespace BankingSystemAssessment.API.Infrastructure
{
    public static class DomainConstants
    {
        public static class Account
        {
            public const int AccountNumberMaxLength = 12;
            public const int CurrencyMaxLength = 10;
            public const int StatusMaxLength = 20;
        }

        public static class Customer
        {            
            public const int CustomerIDLength = 16;
            public const int FirstNameMaxLength = 50;
            public const int LastNameMaxLength = 50;
            public const int MobileMaxLength = 11;
            public const int EmailMaxLength = 256;
            public const int AddressMaxLength = 500;
        }

        public static class Transaction
        {
            public const int ReferenceNumberMaxLength = 20;
            public const int DescriptionMaxLength = 500;
            public const int TranscationTypeMaxLength = 50;
        }
    }
}