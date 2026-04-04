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
            public const decimal DepositMinSum = 10m;
            public const decimal DepositMaxSum = 5000m;
            public const decimal WithdrawMinSum = 50m;
            public const decimal WithdrawMaxSum = 10000m;
        }
    }
}
