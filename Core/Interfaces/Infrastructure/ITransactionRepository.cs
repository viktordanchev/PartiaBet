using Core.Models.Transaction;

namespace Core.Interfaces.Infrastructure
{
    public interface ITransactionRepository
    {
        Task CreateTransactionAsync(CreateTransactionModel data);
    }
}
