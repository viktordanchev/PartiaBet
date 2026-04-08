using Core.Enums;

namespace RestAPI.Dtos.Payments
{
    public class DepositRequestDto
    {
        public decimal Amount { get; set; }

        public TransactionMethod Method { get; set; }

        public object PaymentData { get; set; }
    }
}
