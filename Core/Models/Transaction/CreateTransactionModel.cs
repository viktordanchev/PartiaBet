using Core.Enums;

namespace Core.Models.Transaction
{
    public class CreateTransactionModel
    {
        public Guid Id { get; set; }

        public decimal Amount { get; set; }

        public Guid UserId { get; set; }

        public TransactionType TransactionType { get; set; }
    }
}
