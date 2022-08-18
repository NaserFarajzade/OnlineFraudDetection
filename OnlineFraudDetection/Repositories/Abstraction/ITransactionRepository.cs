using EFDataAccessLibrary.Models;

namespace OnlineFraudDetection.Repositories.Abstraction;

public interface ITransactionRepository
{
    Task AddAsync(Transaction transaction);
    Task AddRangeAsync(IList<Transaction> transactions, int batchSize);
    Task DeleteAll();
    Task<long> GetTransactionAmountAverage(string cardNumber);
}