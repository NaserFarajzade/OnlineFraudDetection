using EFCore.BulkExtensions;
using EFDataAccessLibrary.DataAccess;
using EFDataAccessLibrary.Models;
using OnlineFraudDetection.Repositories.Abstraction;

namespace OnlineFraudDetection.Repositories.Implementation;

public class AccountHolderRepository:IAccountHolderRepository
{
    private readonly DataBaseContext _context;
    private IAccountHolderRepository _accountHolderRepositoryImplementation;

    public AccountHolderRepository(DataBaseContext context)
    {
        _context = context;
    }

    public async Task AddAsync(AccountHolder accountHolder)
    {
        await _context.AccountHolders.AddAsync(accountHolder);
        await _context.SaveChangesAsync();
    }

    public async Task AddRangeAsync(IList<AccountHolder> accountHolders,int batchSize)
    {
        var config = new BulkConfig()
        {
            BatchSize = batchSize
        };
        await _context.BulkInsertAsync(accountHolders,config);
        await _context.BulkSaveChangesAsync(config);
    }

    public async Task DeleteAll()
    {
        _context.RemoveRange(_context.AccountHolders);
        await _context.SaveChangesAsync();
    }

    public Task<IEnumerable<string>> GetAllCardsNumber()
    {
        var cardsNumber = _context.AccountHolders.Select(holder => holder.CardNumber).Distinct().ToList();
        return Task.FromResult<IEnumerable<string>>(cardsNumber);
    }

    public Task<int> GetAccountHolderCardsCount(string cardNumber)
    {
        //TODO Cache piade sazi she
        
        var accountHolder = _context.AccountHolders.First(holder => holder.CardNumber == cardNumber);
        var count = _context.AccountHolders
            .Where(holder => holder.NationalCode == accountHolder.NationalCode)
            .Select(holder => holder.CardNumber)
            .Distinct()
            .Count();
        return Task.FromResult(count);
    }

    public Task<AccountHolder?> Get(string cardNumber)
    {
        var accountHolder = _context.AccountHolders.FirstOrDefault(holder => holder.CardNumber == cardNumber);
        return Task.FromResult(accountHolder);
    }
}