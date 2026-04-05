namespace Common.Constants
{
    public static class Validations
    { 
        public static class User
        {
            public const int EmailMaxLength = 254;
            public const int UsernameMaxLength = 16;
        }

        public static class TransactionLimits
        {
            public const string DepositMinSum = 10m;
            public const string DepositMaxSum = 5000m;
            public const string WithdrawMinSum = 50m;
            public const string WithdrawMaxSum = 10000m;
        }
    }
}
