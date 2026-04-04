using Core.Interfaces.Infrastructure;
using Core.Models.Transaction;
using Infrastructure.Database.Entities;

namespace Infrastructure.Database.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly PartiaBetDbContext _context;

        public TransactionRepository(PartiaBetDbContext context)
        {
            _context = context;
        }

        public async Task CreateTransactionAsync(CreateTransactionModel data)
        {
            var transaction = new Transaction
            {
                Id = data.Id,
                Amount = data.Amount,
                Type = data.TransactionType,
                UserId = data.UserId,
            };

            _context.Add(transaction);
            await _context.SaveChangesAsync();
        }
    }
}
