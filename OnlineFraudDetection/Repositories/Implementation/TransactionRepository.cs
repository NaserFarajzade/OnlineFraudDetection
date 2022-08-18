using EFCore.BulkExtensions;
using EFDataAccessLibrary.DataAccess;
using EFDataAccessLibrary.Models;
using Microsoft.EntityFrameworkCore;
using OnlineFraudDetection.Repositories.Abstraction;

namespace OnlineFraudDetection.Repositories.Implementation;

public class TransactionRepository:ITransactionRepository
{
    private readonly DataBaseContext _context;

    public TransactionRepository(DataBaseContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Transaction transaction)
    {
        await _context.Transactions.AddAsync(transaction);
        await _context.SaveChangesAsync();
    }

    public async Task AddRangeAsync(IList<Transaction> transactions, int batchSize)
    {
        var config = new BulkConfig()
        {
            BatchSize = batchSize
        };
        await _context.BulkInsertAsync(transactions, config);
        await _context.BulkSaveChangesAsync(config);
    }

    public async Task DeleteAll()
    {
        _context.Transactions.RemoveRange(_context.Transactions);
        await _context.SaveChangesAsync();
    }

    public Task<long> GetTransactionAmountAverage(string cardNumber)
    {
        var transactions = _context.Transactions
            .Where(transaction => transaction.OriginCard == cardNumber)
            .Select(transaction => transaction.TransactionAmount)
            .ToList();
        var avg = transactions.Any()? (long)transactions.Average() : 0;
        return Task.FromResult(avg);
    }
}